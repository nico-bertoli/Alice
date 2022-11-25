using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawnState : IPlayerState {

    Vector2 startDirection;
    public PlayerPawnState(Vector2 _startDirection) {

        //caso in cui travestimento preso con alfiere
        if(_startDirection != Vector2.up && _startDirection != Vector2.down && _startDirection != Vector2.right && _startDirection != Vector2.left) {
            if (_startDirection.x > 0) startDirection = Vector2.right;
            else startDirection = Vector2.up;
        }

        startDirection = _startDirection.normalized;
    }

    public void FilterInput(ref Vector2 _dir) {
        if(_dir != startDirection)
            _dir = Vector2.zero;
    }
}
