using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingAreaManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject spottingIndicatorPref;
    float viewSecondsForGameOver = 0.1f;

    private static float? firstPlayerSeenTime = null;

    private SpottingIndicator spottingIndicator;
    private WorldCell cell;
    private Player player;

    private void Awake() {
        GetComponent<MeshRenderer>().enabled = false;
        spottingIndicator = Instantiate(spottingIndicatorPref.GetComponent<SpottingIndicator>());
    }

    private void Start() {
        player = GameController.Instance.Player;
        WorldGrid.Instance.OnGridGenerationCompleted +=
            () => {
                cell = WorldGrid.Instance.GetCellAtPos(transform.position);
                if (cell != null)
                    spottingIndicator.transform.position = cell.Position;
            };
    }

    private void Update() {
        cell = WorldGrid.Instance.GetCellAtPos(transform.position);
        handlePlayerVision();
    }

    public void HardSetArea() {
        spottingIndicator.HardSet(WorldGrid.Instance.GetCellAtPos(transform.position));
    }

    private void handlePlayerVision() {
        if (cell == player.CurrentCell && cell != null) {
            if (firstPlayerSeenTime != null && Time.time - firstPlayerSeenTime >= viewSecondsForGameOver)
                GameController.Instance.GameOver();
            else if (firstPlayerSeenTime == null)
                firstPlayerSeenTime = Time.time;
        }
        else {
            if (Time.time - firstPlayerSeenTime > viewSecondsForGameOver+0.1f)
                firstPlayerSeenTime = null;
        }

            
    }

    //public void TraslateArea() {
    //    Debug.Log("Traslate area called");
    //    spottingIndicator.MoveTo(WorldGrid.Instance.GetCellAtPos(transform.position));
    //}
}

