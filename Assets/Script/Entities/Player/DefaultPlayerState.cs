using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayerState : IPlayerState {
    public void Move(WorldCell _targetCell, WorldCell _currentCell) {
        if (_targetCell == null) {

            WorldCell target = WorldGrid.Instance.GetAdjacentCell(_currentCell, _dir);
            if (target != null) {

                GameObject targetObj = target.CurrentObject;

                if (targetObj != null && targetObj.tag == "Dor")
                    targetObj.GetComponent<Door>().TryOpenDor();

                if (targetObj == null)
                    _targetCell = WorldGrid.Instance.GetAdjacentCell(_currentCell, _dir);
            }
        }
    }
}