using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;


public class Player : GridMover 
{
    [SerializeField] float rewindSeconds = 5f;
    [SerializeField] float rewindCoolDown = 5f;
    [SerializeField] float rewindAnimationSpeed = 4f;

    [SerializeField] Material normalMaterial;
    [SerializeField] Material invisibilityMaterial;


    public bool IsVisible { get; private set; } = true;

    private RewindManager rewindManager;
    private float lastTimeAbilityUsed;
    private bool isRewindActivated = false;
    private MeshRenderer meshRenderer;

    private void Awake() {
        rewindManager = new RewindManager(rewindSeconds);
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Moves player in adjacent cell in given direction (if possible)
    /// </summary>
    /// <param name="_dir">direction to move torwards</param>
    public void MoveToAdjacentCell(WorldGrid.eDirections _dir) {
        if (targetCell == null) {

            WorldCell target = WorldGrid.Instance.GetAdjacentCell(CurrentCell, _dir);
            if (target != null) {

                GameObject targetObj = target.CurrentObject;

                if (targetObj != null && targetObj.tag == "Dor")
                    targetObj.GetComponent<Door>().TryOpenDor();

                if (targetObj == null)
                    targetCell = WorldGrid.Instance.GetAdjacentCell(CurrentCell, _dir);
            }
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
        MoveToTarget();
    }

    /// <summary>
    /// Reads movement input from input manager
    /// </summary>
    private void handleMovementInput() {
        if(targetCell == null && InputManager.Instance.IsMoving) {
            Vector2 input = InputManager.Instance.MoveDirection;
            MoveToAdjacentCell(WorldGrid.VectorToDir(input));

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
        IsVisible = false;
        isRewindActivated = true;

        float originalMoveSpeed = moveSpeed;
        moveSpeed *= rewindAnimationSpeed;
        for(int i = _frames.Count-1; i> 0; i--) {
            targetCell = _frames[i].Cell;
            yield return new WaitForSeconds(((float)_frames[i].EndTime - _frames[i].StratTime)/rewindAnimationSpeed);
        }
        moveSpeed = originalMoveSpeed;

        isRewindActivated=false;
        IsVisible = true;
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


}
