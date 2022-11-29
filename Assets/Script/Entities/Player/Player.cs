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
    public eRoles Disguise = RolesManager.eRoles.PLAYER;

    private void Awake() {
        rewindManager = new RewindManager(rewindSeconds);
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        playerState = new PlayerDefaultState(this);
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
                //playerState = new PlayerPawnState(new Vector2(transform.forward.z, -transform.forward.x));
                break;
            case eRoles.BISHOP:
                playerState = new PlayerBishopState(this);
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
        playerState.RefreshPossibleMoves(this);
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
            Debug.Log("Dress dropped");
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

        public abstract void RefreshPlayerPossibleMovesIndicators();

        protected void makePlayerMoveTorwards(Vector2 _dir) {
            if (player.targetCell == null) {

                WorldCell target = WorldGrid.Instance.GetAdjacentCell(player.currentCell, _dir);
                if (target != null) {

                    GameObject targetObj = target.CurrentObject;

                    if (targetObj != null && targetObj.tag == "Dor")
                        targetObj.GetComponent<Door>().TryOpenDor();

                    if (targetObj != null && targetObj.tag == "PushableBlock")
                        targetObj.GetComponent<PushableBlock>().Push(_dir);

                    if (targetObj == null)
                        player.targetCell = WorldGrid.Instance.GetAdjacentCell(player.currentCell, _dir);
                }
            }
        }
    }

    private class PlayerDefaultState : AbsPlayerState {
        public PlayerDefaultState(Player _player) : base(_player) {}

        public override void Move(ref Vector2 _dir) {
            if (_dir == Vector2.up || _dir == Vector2.right || _dir == Vector2.left || _dir == Vector2.down) makePlayerMoveTorwards(_dir);
        }

        public override void RefreshPlayerPossibleMovesIndicators() {
            player.GetAdjacentCell(Vector2.up)
        }
    }

    private class PlayerBishopState : AbsPlayerState {
        public PlayerBishopState(Player _player) : base(_player) {}

        public override void Move(ref Vector2 _dir) {
            int y = Mathf.RoundToInt(_dir.y);
            int x = Mathf.RoundToInt(_dir.x);

            y = Mathf.Abs(y);
            x = Mathf.Abs(x);

            if (x + y == 2) makePlayerMoveTorwards(_dir);
        }

        public override void RefreshPlayerPossibleMovesIndicators() {
            throw new System.NotImplementedException();
        }
    }

    private class PlayerPawnState : AbsPlayerState {
        private Vector2 startDirection;
        public PlayerPawnState(Player _player, Vector2 _startDirection) : base(_player) {

            //caso in cui travestimento preso con alfiere
            if (_startDirection != Vector2.up && _startDirection != Vector2.down && _startDirection != Vector2.right && _startDirection != Vector2.left) {
                if (_startDirection.x > 0) startDirection = Vector2.right;
                else startDirection = Vector2.up;
            }

            startDirection = _startDirection.normalized;
        }

        public override void Move(ref Vector2 _dir) {
            if (_dir == startDirection)
               makePlayerMoveTorwards(_dir);
        }

        public override void RefreshPlayerPossibleMovesIndicators() {
            throw new System.NotImplementedException();
        }
    }



}
