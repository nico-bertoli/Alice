using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void Move(WorldCell _targetCell, WorldCell _currentCell);
}
