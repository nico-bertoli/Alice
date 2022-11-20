using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : GridMover 
{

    /// <summary>
    /// Moves player in adjacent cell in given direction (if possible)
    /// </summary>
    /// <param name="_dir">direction to move torwards</param>
    public void MoveToAdjacentCell(WorldGrid.eDirections _dir) {
        if (targetCell == null) {
            if(
                WorldGrid.Instance.GetAdjacentCell(CurrentCell, _dir) !=null && 
                WorldGrid.Instance.GetAdjacentCell(CurrentCell, _dir).CurrentObject == null
                )
            targetCell = WorldGrid.Instance.GetAdjacentCell(CurrentCell, _dir);
        }
    }

    protected override void OnCellChanged() { }
    protected override void OnDirectionChanged() { }

    protected override void Update() {
        if(CanMove)
            readMovementInput();

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



}
