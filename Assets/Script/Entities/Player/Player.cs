using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using static RolesManager;

public class Player : GridMover {
    [SerializeField] float rewindSeconds = 5f;
    [SerializeField] float rewindCoolDown = 5f;
    [SerializeField] float rewindAnimationSpeed = 4f;

    [SerializeField] Material normalMaterial;
    [SerializeField] Material invisibilityMaterial;

    [SerializeField] GameObject normalModel;
    [SerializeField] GameObject rewindModel;

    [SerializeField] List<GameObject> possibleMovementIndicators;

    public bool IsVisible { get; private set; } = true;

    private RewindManager rewindManager;
    private float lastTimeAbilityUsed;
    private bool isRewindActivated = false;
    private MeshRenderer meshRenderer;
    private AbsPlayerState playerState;
    public eRoles Disguise = eRoles.PLAYER;

    private void Awake() {
        rewindManager = new RewindManager(rewindSeconds);
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        playerState = new PlayerDefaultState(this);
        for (int i = 0; i < possibleMovementIndicators.Count - 1; i++) possibleMovementIndicators[i].SetActive(true);
    }

    protected override void Start() {
        base.Start();
        WorldGrid.Instance.OnGridGenerationCompleted += playerState.RefreshPossibleMoveIndicators;
    }

    public void SetDisguise(eRoles _disguise) {
        Disguise = _disguise;
        switch (_disguise) {
            case eRoles.PLAYER:
            case eRoles.TOWER:
                playerState = new PlayerDefaultState(this);

                break;
            case eRoles.PAWN:
                playerState = new PlayerPawnState(this,WorldGrid.Instance.ConvertToVectorTwo(transform.forward));
                break;
            case eRoles.BISHOP:
                playerState = new PlayerBishopState(this);
                break;
            case eRoles.HORSE:
                playerState = new PlayerHorseState(this);
                break;
        }
    }

    public WorldCell GetAdjacentCell(Vector2 _dir) {
        return WorldGrid.Instance.GetAdjacentCell(currentCell, _dir);
    }

    public List<GameObject> GetPossibleMovementIndicators(){
        return possibleMovementIndicators;
    }

    protected override void OnCellChanged() {
        rewindManager.RegisterFrame(CurrentCell);
        playerState.RefreshPossibleMoveIndicators();
    }

    protected override void OnDirectionChanged() { }

    protected override void Update() {

        if (isRewindActivated) {
            RotateAwayFromTarget();
        }
        else if (CanMove) {
            handleMovementInput();
            handleRewindInput();
            RotateTorwardsTarget();
        }
        handleDressDrop();
        MoveToTarget();
    }

    private void handleDressDrop() {
        if (InputManager.Instance.IsDroppingDress && Disguise != eRoles.PLAYER) {
            playerState = new PlayerDefaultState(this);
            playerState.RefreshPossibleMoveIndicators();
        }
    }

    /// <summary>
    /// Reads movement input from input manager
    /// </summary>
    private void handleMovementInput() {
        if(targetCell == null && InputManager.Instance.IsMoving) {
            Vector2 input = InputManager.Instance.MoveDirection;
            playerState.Move(ref input);

            //allows rotation torwards walls
            //if (targetCell == null) previousDirection = new Vector3(-input.y,0,input.x);
        }
    }

    /// <summary>
    /// Reads rewind input and handles it
    /// </summary>
    private void handleRewindInput() {
        if(InputManager.Instance.IsUsingAbility && Time.time - lastTimeAbilityUsed >= rewindCoolDown) {
            lastTimeAbilityUsed = Time.time;

            rewindManager.DebugList();

            StartCoroutine(rewind(rewindManager.Rewind()));
        }
    }
    
    private IEnumerator rewind(List<TimeFrame> _frames) {
        setRewindActive(true);

        float originalMoveSpeed = moveSpeed;
        moveSpeed *= rewindAnimationSpeed;
        for(int i = _frames.Count-1; i> 0; i--) {
            targetCell = _frames[i].Cell;
            yield return new WaitForSeconds(((float)_frames[i].EndTime - _frames[i].StratTime)/rewindAnimationSpeed);
        }
        moveSpeed = originalMoveSpeed;

        setRewindActive(false);
    }

    protected override void Init() {
        base.Init();
        rewindManager.RegisterFrame(currentCell);
    }


    public void ActivateInvisibility(float _duration) {
        StartCoroutine(ActivateInvisibilityCor(_duration));
    }

    private IEnumerator ActivateInvisibilityCor(float _duation) {
        IsVisible = false;
        meshRenderer.material = invisibilityMaterial;
        yield return new WaitForSeconds(_duation);
        meshRenderer.material = normalMaterial;
        IsVisible = true;
    }

    private void setRewindActive(bool _enable) {
        IsVisible = !_enable;
        isRewindActivated = _enable;
        rewindModel.SetActive(_enable);
        normalModel.SetActive(!_enable);
    }

    //======================================================================================= states
    private abstract class AbsPlayerState {
        protected Player player;
        public AbsPlayerState (Player _player) { player = _player; }

        /// <summary>
        /// Makes the player move in the given direction
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_dir"></param>
        public abstract void Move(ref Vector2 _dir);

        protected List<Vector2> possibleDirections;

        public void RefreshPossibleMoveIndicators() {
            for (int i = 0; i < possibleDirections.Count; i++) {
                WorldCell targetCell = WorldGrid.Instance.GetAdjacentCell(player.currentCell, possibleDirections[i]);
                if (targetCell) {
                    player.possibleMovementIndicators[i].SetActive(true);
                    player.possibleMovementIndicators[i].transform.position = targetCell.Position;
                }
                else {
                    player.possibleMovementIndicators[i].SetActive(false);
                }
            }

            //deactivating unused indicators
            for (int i = possibleDirections.Count; i < player.possibleMovementIndicators.Count-1; i++)
                player.possibleMovementIndicators[i].SetActive(false);

            //setting allways present indicator under player
            player.possibleMovementIndicators[player.possibleMovementIndicators.Count-1].transform.position = player.currentCell.Position;
        }

        //protected void makePlayerMoveTorwards(Vector2 _dir) {
        //    if (player.targetCell == null) {

        //        WorldCell target = WorldGrid.Instance.GetAdjacentCell(player.currentCell, _dir);
        //        if (target != null) {

        //            GameObject targetObj = target.CurrentObject;

        //            if (targetObj != null && targetObj.tag == "Dor")
        //                targetObj.GetComponent<Door>().TryOpenDor();

        //            if (targetObj != null && targetObj.tag == "PushableBlock")
        //                targetObj.GetComponent<PushableBlock>().Push(_dir);

        //            if (targetObj == null)
        //                player.targetCell = WorldGrid.Instance.GetAdjacentCell(player.currentCell, _dir);
        //        }
        //    }
        //}

        protected Vector2 adjustStartDirection(Vector2 _startDirection) {

            //caso in cui travestimento preso con alfiere
            if (_startDirection != Vector2.up && _startDirection != Vector2.down && _startDirection != Vector2.right && _startDirection != Vector2.left) {
                if (_startDirection.x > 0) _startDirection = Vector2.right;
                else _startDirection = Vector2.up;
            }

            return _startDirection.normalized;
        }

        public List<Vector2> GetDiagonalDirections() {
            return new List<Vector2> { 
                (Vector2.up+Vector2.right).normalized,
                (Vector2.up+Vector2.left).normalized,
                (Vector2.down+Vector2.right).normalized,
                (Vector2.down+Vector2.left).normalized,
            };
        }

        public List<Vector2> GetHorizzontalDirections() {
            return new List<Vector2> { Vector2.up, Vector2.down, Vector2.right, Vector2.left };
        }
    }
    //---------------------------------- default / tower
    private class PlayerDefaultState : AbsPlayerState {

        public PlayerDefaultState(Player _player) : base(_player) {
             possibleDirections = GetHorizzontalDirections();
        }

        public override void Move(ref Vector2 _dir) {
            if (possibleDirections.Contains(_dir)) player.makeMoveTorwardsDirection(_dir);
        }

        
    }
    //---------------------------------- bishop
    private class PlayerBishopState : AbsPlayerState {
        public PlayerBishopState(Player _player) : base(_player) {
            possibleDirections = GetDiagonalDirections();
        }

        public override void Move(ref Vector2 _dir) {
            //int y = Mathf.RoundToInt(_dir.y);
            //int x = Mathf.RoundToInt(_dir.x);

            //y = Mathf.Abs(y);
            //x = Mathf.Abs(x);

            //if (x + y == 2) makePlayerMoveTorwards(_dir);
            if (possibleDirections.Contains(_dir.normalized)) player.makeMoveTorwardsDirection(_dir);
        }
    }
    //---------------------------------- pawn
    private class PlayerPawnState : AbsPlayerState {
        private Vector2 startDirection;
        public PlayerPawnState(Player _player, Vector2 _startDirection) : base(_player) {

            startDirection = adjustStartDirection(_startDirection);

            possibleDirections = new List<Vector2> { startDirection };
            }

        public override void Move(ref Vector2 _dir) {
            if (_dir == startDirection)
               player.makeMoveTorwardsDirection(_dir);
        }
    }
    //---------------------------------- horse
    private class PlayerHorseState : AbsPlayerState {

        private bool isHorizzontalPhase;
        private Vector2 lastHorizDirection;

        public PlayerHorseState(Player _player) : base(_player) {
            isHorizzontalPhase = true;
            possibleDirections = GetHorizzontalDirections();
        }

        public override void Move(ref Vector2 _dir) {

            if (possibleDirections.Contains(_dir.normalized)) {
                player.makeMoveTorwardsDirection(_dir);
                if(isHorizzontalPhase)lastHorizDirection = _dir;
                switchPhase();
            }
        }

        private void switchPhase() {
            isHorizzontalPhase = !isHorizzontalPhase;
            if (isHorizzontalPhase) {
                possibleDirections = GetHorizzontalDirections();
            }
            else {
                possibleDirections = GetDiagonalDirections();
                List<Vector2> toRemove = new List<Vector2>();
                for(int i = 0; i < possibleDirections.Count; i++) {
                    if (
                        Mathf.Sign(possibleDirections[i].x) != Mathf.Sign(lastHorizDirection.x) && lastHorizDirection.x != 0
                        ||
                        Mathf.Sign(possibleDirections[i].y) != Mathf.Sign(lastHorizDirection.y) && lastHorizDirection.y != 0
                        )
                        toRemove.Add(possibleDirections[i]);
                }
                foreach (Vector2 vect in toRemove) possibleDirections.Remove(vect);
                    
            }
        }
    }



}
