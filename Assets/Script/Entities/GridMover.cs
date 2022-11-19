using UnityEngine;

/// <summary>
/// Object that can move on world grid
/// </summary>
public class GridMover : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] private float rotationSpeed;

    protected WorldCell currentCell;
    protected WorldCell targetCell;

    protected virtual void Start() {
        WorldGrid.Instance.OnGridGenerationCompleted += InitPosition;
    }

    protected virtual void Update() {
        MoveToTarget();
        RotateTorwardsTarget();
    }

    /// <summary>
    /// Moves the object torwards target cell
    /// </summary>
    private void MoveToTarget() {
        if (targetCell) {
            transform.position = Vector3.MoveTowards(transform.position, targetCell.Position, moveSpeed * Time.deltaTime);
            currentCell = WorldGrid.Instance.GetCellAtPos(transform.position);
            if(Vector3.Distance(transform.position, targetCell.Position) < Mathf.Epsilon) {
                transform.position = targetCell.Position;
                targetCell = null;
            }
        }
    }

    /// <summary>
    /// Setup object position on starting cell
    /// </summary>
    /// <returns></returns>
    private void InitPosition() {
        currentCell = WorldGrid.Instance.GetCellAtPos(transform.position);
        transform.position = currentCell.Position;
    }

    private void RotateTorwardsTarget() {
        if (targetCell) {
            Vector3 forward = (targetCell.Position - transform.position).normalized;
            if (forward != Vector3.zero) {
                Quaternion toRot = Quaternion.LookRotation(forward, transform.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
