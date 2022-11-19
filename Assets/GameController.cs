using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private Player player;

    public Player Player { get { return player; } }
    
    public void GameOver() {
        Debug.Log("Game Over!");
    }
}
