using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : GridMover
{
    public void Push(Vector2 _dir) {
        targetCell = WorldGrid.Instance.GetAdjacentCell(currentCell, _dir);
        if (targetCell.CurrentObject != null) targetCell = null;
    }

    protected override void OnCellChanged() {}
    protected override void OnDirectionChanged() {}
}
