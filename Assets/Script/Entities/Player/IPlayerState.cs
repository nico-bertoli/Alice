using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void Move(ref WorldCell _targetCell, ref WorldCell _currentCell,Vector2 _dir);
}
