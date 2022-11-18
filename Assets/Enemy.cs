using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GridMover
{
    [SerializeField] Transform wayPointsContainer;
    
    private List<Transform> wayPoints;
    private int wayPointIndex = 1;

    private void Awake() {
        WorldGrid.Instance.OnGridGenerationCompleted += Init;
    }

    protected override void Update() {
        SetupTarget();
        base.Update();
    }

    private void SetupTarget() {
        if(wayPoints != null && wayPoints.Count > 0 && target == null) {
            Debug.Log("setup target");
            target = WorldGrid.Instance.GetCellAtPos(wayPoints[wayPointIndex++].position);
            if (wayPointIndex == wayPoints.Count)
                wayPointIndex = 0;
        }
    }

    private void Init() {
        wayPoints = new List<Transform>();
        for (int i = 0; i < wayPointsContainer.childCount; i++)
            wayPoints.Add(wayPointsContainer.GetChild(i));
    }
}
