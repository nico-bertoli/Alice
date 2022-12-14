using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Platform : MonoBehaviour
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
                otherMover.CanRotate = false;
                otherMover.makeMoveTorwardsDirection(DefineDirection(otherMover));

                Player otherPlayer = cell.CurrentObject.GetComponent<Player>();
                if (otherPlayer) otherPlayer.EnablePossibleMovesIndicators = false;
            }
        }
    }

    protected abstract Vector2 DefineDirection(GridMover other);

}
