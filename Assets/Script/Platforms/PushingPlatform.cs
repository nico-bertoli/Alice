using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingPlatform : Platform {
    protected override Vector2 DefineDirection(GridMover other) {
        return WorldGrid.Instance.ConvertToVectorTwo(transform.forward);
    }
}
