using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMover : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected WorldCell cell;
    protected WorldCell target;

    private void Start() {
        cell = WorldGrid.Instance.GetCellAtPos(transform.position);
    }

    //private IEnumerator InitPosition()

    public void SetNewTarget(WorldGrid.eDirections _dir) {
        if (!target) {
            target = WorldGrid.Instance.GetAdjacentCell(cell, _dir);
        }
    }

    protected virtual void Update() {
        MoveToTarget();
    }

    private void MoveToTarget() {
        if (target != null) {
            transform.position = Vector3.MoveTowards(transform.position, cell.Position, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, target.Position) < Mathf.Epsilon) {
                transform.position = target.Position;
                cell = target;
                target = null;
            }
        }
    }
}
