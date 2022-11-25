using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RolesManager;

public class SpottingAreaManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject spottingIndicatorPref;
    [SerializeField] bool debug;

    private SpottingArea spottingArea;
    private WorldCell cell;

    private void Awake() {
        GetComponent<MeshRenderer>().enabled = false;
        spottingArea = Instantiate(spottingIndicatorPref.GetComponent<SpottingArea>());
    }

    private void Start() {
        WorldGrid.Instance.OnGridGenerationCompleted +=
            () => {
                cell = WorldGrid.Instance.GetCellAtPos(transform.position);
                if (cell != null)
                    spottingArea.transform.position = cell.Position;
            };
    }

    private void Update() {
        cell = WorldGrid.Instance.GetCellAtPos(transform.position);
    }

    public void HardSetArea() {
        spottingArea.SetCell(WorldGrid.Instance.GetCellAtPos(transform.position));
    }

    public void SetType(eRoles _type) {
        spottingArea.Role = _type;
    }
}

