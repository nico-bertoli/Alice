using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingArea : MonoBehaviour
{
    [SerializeField] GameObject model;
    private WorldCell cell;
    private Player player;
    private static float? firstPlayerSeenTime = null;

    public Player.eRoles Type { get; set;}

    private const float VIEW_SECONDS_FOR_GAMEOVER = 0.05f;

    private void Start() {
        player = GameController.Instance.Player;
        WorldGrid.Instance.OnGridGenerationCompleted +=
           () => { SetCell(WorldGrid.Instance.GetCellAtPos(transform.position)); };
        
    }

    public void SetCell(WorldCell _cell) {
        if (_cell != null) {
            cell = _cell;
            transform.position = _cell.Position;
            model.SetActive(true);
        }
        else model.SetActive(false);
    }

    private void Update() {
        handlePlayerVision();
    }

    private void handlePlayerVision() {
        if (cell == player.CurrentCell && cell != null && player.IsVisible && hasPlayerLowerHierarchy()) {
            if (firstPlayerSeenTime != null && Time.time - firstPlayerSeenTime >= VIEW_SECONDS_FOR_GAMEOVER)
                GameController.Instance.GameOver();
            else if (firstPlayerSeenTime == null)
                firstPlayerSeenTime = Time.time;
        }
        else {
            if (Time.time - firstPlayerSeenTime > VIEW_SECONDS_FOR_GAMEOVER + 0.1f)
                firstPlayerSeenTime = null;
        }
    }

    private bool hasPlayerLowerHierarchy() {
        if (player.Disguise < Type) return true;
        return false;
    }

}
