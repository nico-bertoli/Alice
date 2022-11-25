using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Player : GridMover 
{
    [SerializeField] float rewindSeconds = 5f;
    [SerializeField] float rewindCoolDown = 5f;
    [SerializeField] float rewindAnimationSpeed = 4f;

    [SerializeField] Material normalMaterial;
    [SerializeField] Material invisibilityMaterial;

    [SerializeField] GameObject normalModel;
    [SerializeField] GameObject rewindModel;

    public enum eDisguises {NO_DISGUISE,PAWN,TOWER,BISHOP}
    public bool IsVisible { get; private set; } = true;

    private RewindManager rewindManager;
    private float lastTimeAbilityUsed;
    private bool isRewindActivated = false;
    private MeshRenderer meshRenderer;
    private IPlayerState playerState;
    public eDisguises Disguise = eDisguises.NO_DISGUISE;

    private void Awake() {
        rewindManager = new RewindManager(rewindSeconds);
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        playerState = new PlayerDefaultState();
    }

    public void SetDisguise(eDisguises _disguise) {
        Disguise = _disguise;
        switch (_disguise) {
            case eDisguises.NO_DISGUISE:
            case eDisguises.TOWER:
                playerState = new PlayerDefaultState();

                break;
            case eDisguises.PAWN:
                playerState = new PlayerPawnState(new Vector2(transform.forward.z, -transform.forward.x));
                break;
            case eDisguises.BISHOP:
                playerState = new PlayerBishopState();
                break;
        }
    }

    protected override void OnCellChanged() {
        rewindManager.RegisterFrame(CurrentCell);
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
        if (InputManager.Instance.IsDroppingDress && Disguise != eDisguises.NO_DISGUISE) {
                playerState = new PlayerDefaultState();
            Debug.Log("Dress dropped");
        }
    }
    
    private void move(Vector2 _dir) {
        if (targetCell == null) {

            Debug.Log(_dir);

            WorldCell target = WorldGrid.Instance.GetAdjacentCell(currentCell, _dir);
            if (target != null) {

                GameObject targetObj = target.CurrentObject;

                if (targetObj != null && targetObj.tag == "Dor")
                    targetObj.GetComponent<Door>().TryOpenDor();

                if (targetObj == null)
                    targetCell = WorldGrid.Instance.GetAdjacentCell(currentCell, _dir);
            }
        }
    }

    /// <summary>
    /// Reads movement input from input manager
    /// </summary>
    private void handleMovementInput() {
        if(targetCell == null && InputManager.Instance.IsMoving) {
            Vector2 input = InputManager.Instance.MoveDirection;
            playerState.FilterInput(ref input);
            move(input);
            //allows rotation torwards walls
            if (targetCell == null) previousDirection = new Vector3(-input.y,0,input.x);
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
        Debug.Log("invisible");
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

}
