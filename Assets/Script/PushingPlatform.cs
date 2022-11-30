using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingPlatform : MonoBehaviour
{
    private WorldCell cell;

    private void Start() {
        WorldGrid.Instance.OnGridGenerationCompleted += () => {
            cell = WorldGrid.Instance.GetCellAtPos(transform.position);
            cell.OnCurrentObjectChange += pushObject;
        };
    }

    private void pushObject() {
        if (cell.CurrentObject) {
            GridMover otherMover = cell.CurrentObject.GetComponent<GridMover>();
            if (otherMover) {
                Debug.Log("PUSH OBJECT");
                otherMover.makeMoveTorwardsDirection(WorldGrid.Instance.ConvertToVectorTwo(transform.forward));
            }
        }
    }
}
