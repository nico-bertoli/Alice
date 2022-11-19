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
            targetCell = WorldGrid.Instance.GetAdjacentCell(currentCell, _dir);
        }
    }

    protected override void Update() {
        readMovementInput();
        base.Update();
    }

    /// <summary>
    /// Reads movement input from input manager
    /// </summary>
    private void readMovementInput() {
        if(targetCell == null && InputManager.Instance.IsMoving) {
            MoveToAdjacentCell(WorldGrid.VectorToDir(InputManager.Instance.MoveDirection));
        }
    }



}
