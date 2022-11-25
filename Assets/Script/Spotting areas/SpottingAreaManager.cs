using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingAreaManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject spottingIndicatorPref;
    [SerializeField] bool debug;

    private SpottingArea spottingIndicator;
    private WorldCell cell;
    private Player player;

    private void Awake() {
        GetComponent<MeshRenderer>().enabled = false;
        spottingIndicator = Instantiate(spottingIndicatorPref.GetComponent<SpottingArea>());
    }

    private void Start() {
        WorldGrid.Instance.OnGridGenerationCompleted +=
            () => {
                cell = WorldGrid.Instance.GetCellAtPos(transform.position);
                if (cell != null)
                    spottingIndicator.transform.position = cell.Position;
            };
    }

    private void Update() {
        cell = WorldGrid.Instance.GetCellAtPos(transform.position);
    }

    public void HardSetArea() {
        spottingIndicator.SetCell(WorldGrid.Instance.GetCellAtPos(transform.position));
    }
}

