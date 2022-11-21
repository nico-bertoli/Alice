using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : GridMover 
{
    [SerializeField] float rewindSeconds = 5f;
    [SerializeField] float rewindCoolDown = 5f;

    RewindManager rewindManager;
    private float lastTimeAbilityUsed;

    private void Awake() {
        rewindManager = new RewindManager(rewindSeconds);
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
        if (CanMove) {
            readMovementInput();
            readRewindInput();
        }
        base.Update();
    }

    /// <summary>
    /// Reads movement input from input manager
    /// </summary>
    private void readMovementInput() {
        if(targetCell == null && InputManager.Instance.IsMoving) {
            Vector2 input = InputManager.Instance.MoveDirection;
            MoveToAdjacentCell(WorldGrid.VectorToDir(input));

            //allows rotation torwards walls
            if (targetCell == null) previousDirection = new Vector3(-input.y,0,input.x);
        }
    }

    private void readRewindInput() {
        if(InputManager.Instance.IsUsingAbility && Time.time - lastTimeAbilityUsed >= rewindCoolDown) {
            Debug.Log("Ability used");
            lastTimeAbilityUsed = Time.time;
            
            currentCell.CurrentObject = null;
            currentCell = rewindManager.Rewind();
            if (currentCell.CurrentObject == null)
                currentCell.CurrentObject = gameObject;
            transform.position = currentCell.Position;
            
        }
    }



}
