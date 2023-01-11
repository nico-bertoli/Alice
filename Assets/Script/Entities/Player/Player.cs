using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] GameObject AliceModel;
    [SerializeField] GameObject PedoneModel;
    [SerializeField] GameObject TorreModel;
    [SerializeField] GameObject AlfiereModel;
    [SerializeField] GameObject CavalloModel;
    [SerializeField] GameObject ResizedModel;
    [SerializeField] GameObject InvisibleModel;

    [SerializeField] GameObject Camera;
    [SerializeField] GameObject Camera1;
    [SerializeField] GameObject Camera2;
    [SerializeField] GameObject Camera3;
    [SerializeField] GameObject Camera4;

    public bool IsVisible { get; private set; } = true;
    public bool EnablePossibleMovesIndicators = true;

    private RewindManager rewindManager;
    private float lastTimeAbilityUsed;
    private bool isRewindActivated = false;
    private MeshRenderer meshRenderer;
    private AbsPlayerState playerState;
    public eRoles Disguise = eRoles.PLAYER;
    public Canvas PausePanel;
    //public GameObject UiPanel;
    //UIController uiscript;
    PauseControl pausescript;
    private int CurrentCamera = 0;

    private void Awake() {
        rewindManager = new RewindManager(rewindSeconds);
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        playerState = new PlayerDefaultState(this);
        for (int i = 0; i < possibleMovementIndicators.Count - 1; i++) possibleMovementIndicators[i].SetActive(true);
        pausescript = PausePanel.GetComponent<PauseControl>();
        //uiscript = UiPanel.GetComponent<UIController>();
    }

    protected override void Start() {
        base.Start();
        WorldGrid.Instance.OnGridGenerationCompleted += playerState.RefreshPossibleMoveIndicators;
        getDisguise();
    }

    public void SetDisguise(eRoles _disguise) {
        Disguise = _disguise;
        switch (_disguise) {
            case eRoles.PLAYER:
            case eRoles.TOWER:
                playerState = new PlayerDefaultState(this);
                resetModels();
                TorreModel.SetActive(true);
                break;
            case eRoles.PAWN:
                playerState = new PlayerPawnState(this,WorldGrid.Instance.ConvertToVectorTwo(transform.forward));
                resetModels();
                PedoneModel.SetActive(true);
                break;
            case eRoles.BISHOP:
                playerState = new PlayerBishopState(this);
                resetModels();
                AlfiereModel.SetActive(true);
                break;
            case eRoles.HORSE:
                playerState = new PlayerHorseState(this);
                resetModels();
                CavalloModel.SetActive(true);
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
        if (!EnablePossibleMovesIndicators) EnablePossibleMovesIndicators = true;
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
        handleCameraLeft();
        handleCameraRight();
        handlePauseWithPKey();
    }

    private void handlePauseWithPKey()
    {
        if (InputManager.Instance.IsPaused && !GameController.Instance.IsOver && !GameController.Instance.PlayerWon)
        {
            pausescript.ShowPause();
            //Debug.Log(InputManager.Instance.IsPaused + " " + GameController.Instance.IsOver + " " + GameController.Instance.PlayerWon);
        }
    }

    private void handleDressDrop() {
        if (InputManager.Instance.IsDroppingDress && Disguise != eRoles.PLAYER) {
            playerState = new PlayerDefaultState(this);
            playerState.RefreshPossibleMoveIndicators();
            resetModels();
            AliceModel.SetActive(true);
            Disguise = eRoles.PLAYER;
        }
    }

    void resetModels()
    {
        AliceModel.SetActive(false);
        TorreModel.SetActive(false);
        AlfiereModel.SetActive(false);
        CavalloModel.SetActive(false);
        PedoneModel.SetActive(false);
        ResizedModel.SetActive(false);
        InvisibleModel.SetActive(false);
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
        for(int i = _frames.Count-1; i>= 0; i--) {
            targetCell = _frames[i].Cell;
            yield return new WaitForSeconds(((float)_frames[i].EndTime - _frames[i].StratTime)/rewindAnimationSpeed);
        }
        moveSpeed = originalMoveSpeed;

        setRewindActive(false);
        getDisguise();
    }

    protected override void Init() {
        base.Init();
        rewindManager.RegisterFrame(currentCell);
    }


    public void ActivateInvisibility(float _duration) {
        StartCoroutine(ActivateInvisibilityCor(_duration));
    }

    public void ActivateResizing(float _duration)
    {
        StartCoroutine(ActivateResizingCor(_duration));
    }

    private IEnumerator ActivateInvisibilityCor(float _duation) {
        IsVisible = false;
        meshRenderer.material = invisibilityMaterial;
        resetModels();
        InvisibleModel.SetActive(true);
        yield return new WaitForSeconds(_duation);
        meshRenderer.material = normalMaterial;
        getDisguise();
        IsVisible = true;
    }

    private IEnumerator ActivateResizingCor(float _duation)
    {
        resetModels();
        ResizedModel.SetActive(true);
        yield return new WaitForSeconds(_duation);
        ResizedModel.SetActive(false);
        AliceModel.SetActive(true);
        getDisguise();
    }


    void getDisguise()
    {
        resetModels();
        
        switch (Disguise)
        {
            case eRoles.PLAYER:                
                AliceModel.SetActive(true);
                break;
            case eRoles.TOWER:
                TorreModel.SetActive(true);
                break;
            case eRoles.PAWN:
                PedoneModel.SetActive(true);
                break;
            case eRoles.BISHOP:
                AlfiereModel.SetActive(true);
                break;
            case eRoles.HORSE:
                CavalloModel.SetActive(true);
                break;
        }
    }

    private void setRewindActive(bool _enable) {
        isRewindActivated = _enable;
        if (_enable)
            resetModels();
        IsVisible = !_enable;
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
            if (player.EnablePossibleMovesIndicators) {
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
                for (int i = possibleDirections.Count; i < player.possibleMovementIndicators.Count - 1; i++)
                    player.possibleMovementIndicators[i].SetActive(false);

                //setting always present indicator under player
                player.possibleMovementIndicators[player.possibleMovementIndicators.Count - 1].transform.position = player.currentCell.Position;
                player.possibleMovementIndicators[player.possibleMovementIndicators.Count - 1].SetActive(true);
            }
            else foreach (GameObject indicator in player.possibleMovementIndicators) indicator.SetActive(false);
        }

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

    // =============================================================== camera

    private void handleCameraLeft()
    {
        if (InputManager.Instance.IsCameraRotatingLeft && !InputManager.Instance.IsCameraLeftUpdated)
        {
            CurrentCamera = (CurrentCamera - 1) % 4;
            updateCamera();
            InputManager.Instance.IsCameraLeftUpdated = true;
            // Camera.gameObject.transform.Rotate(45.0f, 0.0f, 0.0f);
        } else if (!InputManager.Instance.IsCameraRotatingLeft)
        {
            InputManager.Instance.IsCameraLeftUpdated = false;
        }
    }

    private void handleCameraRight()
    {
        if (InputManager.Instance.IsCameraRotatingRight && !InputManager.Instance.IsCameraRightUpdated)
        {
            CurrentCamera = (CurrentCamera + 1) % 4;
            updateCamera();
            InputManager.Instance.IsCameraRightUpdated = true;
            // Camera.gameObject.transform.Rotate(0.0f, -45.0f, 0.0f);
        }
        else if (!InputManager.Instance.IsCameraRotatingRight)
        {
            InputManager.Instance.IsCameraRightUpdated = false;
        }
    }

    private void updateCamera()
    {
        if (CurrentCamera < 0) CurrentCamera = CurrentCamera + 4;
        switch (CurrentCamera)
        {
            case 0:
                Camera2.SetActive(false);
                Camera3.SetActive(false);
                Camera4.SetActive(false);
                Camera1.SetActive(true);
                break;
            case 1:
                Camera1.SetActive(false);
                Camera3.SetActive(false);
                Camera4.SetActive(false);
                Camera2.SetActive(true);
                break;
            case 2:
                Camera1.SetActive(false);
                Camera2.SetActive(false);
                Camera4.SetActive(false);
                Camera3.SetActive(true);
                break;
            case 3:
                Camera1.SetActive(false);
                Camera2.SetActive(false);
                Camera3.SetActive(false);
                Camera4.SetActive(true);
                break;
            default:
                break;
        }
    }

}
