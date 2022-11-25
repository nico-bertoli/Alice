using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : GridMover
{
    //protected override void Start() {
    //    base.Start();
    //    Init();
    //}
    public void Push(Vector2 _dir) {
        Debug.Log("push called with dir: "+_dir);
        targetCell = WorldGrid.Instance.GetAdjacentCell(currentCell, _dir);
        Debug.Log("current: "+currentCell+"target: "+targetCell);
    }

    protected override void OnCellChanged() {}
    protected override void OnDirectionChanged() {}
}
