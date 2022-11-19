using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GridMover
{
    [SerializeField] Transform wayPointsContainer;
    
    /// <summary>
    /// waypoints list
    /// </summary>
    private List<Transform> waypoints;
    /// <summary>
    /// next waypoint index
    /// </summary>
    private int wayPointIndex = 1;

    protected override void Start() {
        WorldGrid.Instance.OnGridGenerationCompleted += Init;
    }

    protected override void Update() {
        SetupTarget();
        base.Update();
    }

    /// <summary>
    /// if current waypoint was reached, sets next waypoint as target
    /// </summary>
    private void SetupTarget() {
        if(waypoints != null && waypoints.Count > 0 && targetCell == null) {
            Debug.Log("setup target");
            targetCell = WorldGrid.Instance.GetCellAtPos(waypoints[wayPointIndex++].position);
            if (wayPointIndex == waypoints.Count)
                wayPointIndex = 0;
        }
    }

    /// <summary>
    /// Initializes the object
    /// </summary>
    private void Init() {
        waypoints = new List<Transform>();
        for (int i = 0; i < wayPointsContainer.childCount; i++)
            waypoints.Add(wayPointsContainer.GetChild(i));
    }
}
