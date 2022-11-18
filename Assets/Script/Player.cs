using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GridMover
{

    protected override void Update() {
        readInput();
        base.Update();
    }

    private void readInput() {
        if(target == null && InputManager.Instance.IsMoving) {
            SetNewTarget(WorldGrid.VectorToDir(InputManager.Instance.MoveDirection));
        }
    }

}
