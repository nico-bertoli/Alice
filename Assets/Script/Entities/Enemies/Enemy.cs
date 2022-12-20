using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RolesManager;

public class Enemy : GridMover
{
    [SerializeField] Transform wayPointsContainer;
    [SerializeField] Transform SpottingManagersContainer;
    
    /// <summary>
    /// waypoints list
    /// </summary>
    private List<Transform> waypoints;
    private List<SpottingAreaManager> spottingManagers;

    [SerializeField] eRoles type;

    /// <summary>
    /// next waypoint index
    /// </summary>
    private int wayPointIndex = 1;

    protected override void Start() {
        base.Start();
        //WorldGrid.Instance.OnGridGenerationCompleted += Init;
        GameController.Instance.RegisterEnemy(this);
    }

    protected override void Update() {
        if(Time.timeScale != 0) Debug.Log(targetCell);

        SetupTarget();

        if(CanMove)
            MoveToTarget();
        else
            refreshSpottingAreas();

        RotateTorwardsTarget();
    }

    /// <summary>
    /// if current waypoint was reached, sets next waypoint as target
    /// </summary>
    private void SetupTarget() {
        if(waypoints != null && waypoints.Count > 0 && targetCell == null) {
            Debug.Log("setup target");

            wayPointIndex++;
            if (wayPointIndex == waypoints.Count)wayPointIndex = 0;

            targetCell = WorldGrid.Instance.GetCellAtPos(waypoints[wayPointIndex].position);
            
        }
    }

    /// <summary>
    /// Initializes the object
    /// </summary>
    protected override void Init() {
        base.Init();
        waypoints = new List<Transform>();
        for (int i = 0; i < wayPointsContainer.childCount; i++)
            waypoints.Add(wayPointsContainer.GetChild(i));

        spottingManagers = new List<SpottingAreaManager>();
        for (int i = 0; i < SpottingManagersContainer.childCount; i++) {
            spottingManagers.Add(SpottingManagersContainer.GetChild(i).GetComponent<SpottingAreaManager>());
            spottingManagers[spottingManagers.Count - 1].SetType(type);
        }
            
    }

    protected override void OnDirectionChanged() {
        refreshSpottingAreas();
    }

    protected override void OnCellChanged() {
        refreshSpottingAreas();
    }

    private void refreshSpottingAreas() {
        if (spottingManagers != null) {
            foreach (SpottingAreaManager manager in spottingManagers)
                manager.HardSetArea();
        }
    }
}
