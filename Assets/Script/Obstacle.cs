using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : GridMover
{

    public void NoMove(Vector2 _dir)
    {
        targetCell = WorldGrid.Instance.GetAdjacentCell(currentCell, _dir);
        if (targetCell != null) targetCell = null;
        //else targetCell = null;
    }

    protected override void OnCellChanged() { }
    protected override void OnDirectionChanged() { }
}




