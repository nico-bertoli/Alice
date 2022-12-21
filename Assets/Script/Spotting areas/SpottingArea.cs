using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RolesManager;

public class SpottingArea : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float spotSeconds = 0.5f;
    [Header("References")]
    [SerializeField] GameObject model;
    private WorldCell cell;
    private Player player;
    private static float? firstPlayerSeenTime = null;

    public eRoles Role { get; set;}

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
        if (cell == player.CurrentCell && cell != null && player.IsVisible && IsRoleLowerThan(player.Disguise,Role)) {
            if (firstPlayerSeenTime != null && Time.time - firstPlayerSeenTime >= spotSeconds)
                GameController.Instance.GameOver();
            else if (firstPlayerSeenTime == null)
                firstPlayerSeenTime = Time.time;

            if (firstPlayerSeenTime != null) Debug.Log(Time.time - firstPlayerSeenTime);
        }
        else {
            if (Time.time - firstPlayerSeenTime > spotSeconds + 0.1f)
                firstPlayerSeenTime = null;
        }
    }

    

}
