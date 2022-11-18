using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMover : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected WorldCell cell;
    protected WorldCell target;

    private void Awake() {
        WorldGrid.Instance.OnGridGenerationCompleted += InitPosition;
    }

    protected virtual void Update() {
        MoveToTarget();
    }

    private void MoveToTarget() {
        if (target) {
            transform.position = Vector3.MoveTowards(transform.position, target.Position, speed * Time.deltaTime);
            cell = WorldGrid.Instance.GetCellAtPos(transform.position);
            if(Vector3.Distance(transform.position, target.Position) < Mathf.Epsilon) {
                transform.position = target.Position;
                target = null;
            }
        }
    }

    /// <summary>
    /// Centers the object in its starting cell
    /// </summary>
    /// <returns></returns>
    private void InitPosition() {
        cell = WorldGrid.Instance.GetCellAtPos(transform.position);
        transform.position = cell.Position;
    }
}
