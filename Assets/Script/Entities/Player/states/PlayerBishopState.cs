using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBishopState : IPlayerState {
    public void FilterInput(ref Vector2 _dir) {

        int y = Mathf.RoundToInt(_dir.y);
        int x = Mathf.RoundToInt(_dir.x);

        y = Mathf.Abs(y);
        x = Mathf.Abs(x);

        if (x + y != 2) _dir = Vector2.zero;
    }
}
