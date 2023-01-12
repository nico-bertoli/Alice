using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RolesManager;

public class SpottingArea : MonoBehaviour
{
    [SerializeField] GameObject model;
    private WorldCell cell;
    private Player player;
    private static float? firstPlayerSeenTime = null;

    public eRoles Role { get; set;}

    public float VIEW_SECONDS_FOR_GAMEOVER ;

    private void Start() {
        player = GameController.Instance.Player;
        WorldGrid.Instance.OnGridGenerationCompleted +=
           () => { SetCell(WorldGrid.Instance.GetCellAtPos(transform.position)); };
        
    }

    public void SetCell(WorldCell _cell)
    {
        model.SetActive(false);
        if (_cell != null)
        {
            cell = _cell;
            transform.position = _cell.Position;
            GameObject targetObj = cell.CurrentObject;
            if (targetObj != null && targetObj.CompareTag("Obstacle"))
            {
                model.SetActive(false);
                //Debug.Log("spottingarea " + targetObj.tag);
            }
            else
                model.SetActive(true);
        }
    }

    private void Update() {
        handlePlayerVision();
    }

    private void handlePlayerVision() {
        if (cell == player.CurrentCell && cell != null && player.IsVisible && IsRoleLowerThan(player.Disguise,Role)) {
            if (firstPlayerSeenTime != null)
                { if (Time.time - firstPlayerSeenTime >= VIEW_SECONDS_FOR_GAMEOVER)
                GameController.Instance.GameOver(); }
            else if (firstPlayerSeenTime == null)
                firstPlayerSeenTime = Time.time;

            if (firstPlayerSeenTime != null) Debug.Log(Time.time - firstPlayerSeenTime);
        }
        else {
            if (Time.time - firstPlayerSeenTime > VIEW_SECONDS_FOR_GAMEOVER + 0.1f)
                firstPlayerSeenTime = null;
        }
    }

    

}
