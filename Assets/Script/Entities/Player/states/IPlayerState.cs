using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void FilterInput(ref Vector2 _dir);
}
