using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultState : IPlayerState {
    public void FilterInput(ref Vector2 _dir) {
        if (_dir != Vector2.up && _dir != Vector2.right && _dir != Vector2.left && _dir != Vector2.down) _dir = Vector2.zero;
    }
}
