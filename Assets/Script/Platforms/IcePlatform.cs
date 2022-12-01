using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatform : Platform {
    protected override Vector2 DefineDirection(GridMover other) {
        Debug.Log(WorldGrid.Instance.ConvertToVectorTwo(other.gameObject.transform.forward));
        return WorldGrid.Instance.ConvertToVectorTwo(other.gameObject.transform.forward);
    }
}
