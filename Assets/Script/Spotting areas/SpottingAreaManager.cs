using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingAreaManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject spottingIndicatorPref;
    [SerializeField] bool debug;

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
        if (cell == player.CurrentCell && cell!=null)
            GameController.Instance.GameOver();
    }

    public void HardSetArea() {
        spottingIndicator.HardSet(WorldGrid.Instance.GetCellAtPos(transform.position));
        if (debug) {
            Debug.Log("hard set called with: position: " + transform.position + ", cell returned: " + WorldGrid.Instance.GetCellAtPos(transform.position));
        }
    }

    //public void TraslateArea() {
    //    Debug.Log("Traslate area called");
    //    spottingIndicator.MoveTo(WorldGrid.Instance.GetCellAtPos(transform.position));
    //}
}

