using UnityEngine;

/// <summary>
/// Object that can move on world grid
/// </summary>
public abstract class GridMover : GridObject
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] private float rotationSpeed;

    public bool CanMove { get; set; } = false;
    protected WorldCell targetCell;
    private WorldCell previousCell;
    private Vector3 previousDirection;

    protected virtual void Update() {
        if (CanMove) {
            MoveToTarget();
            RotateTorwardsTarget();
        }
    }

    /// <summary>
    /// Moves the object torwards target cell
    /// </summary>
    protected void MoveToTarget() {
        if (targetCell) {
            transform.position = Vector3.MoveTowards(transform.position, targetCell.Position, moveSpeed * Time.deltaTime);
            currentCell = WorldGrid.Instance.GetCellAtPos(transform.position);
            if(Vector3.Distance(transform.position, targetCell.Position) < Mathf.Epsilon) {
                transform.position = targetCell.Position;
                targetCell = null;
            }
            if (previousCell != CurrentCell) {
                if(previousCell)previousCell.Walkable = true;
                previousCell = CurrentCell;
                CurrentCell.Walkable = false;
                OnCellChanged();
            }
        }
    }

    /// <summary>
    /// Setup object position on starting cell
    /// </summary>
    /// <returns></returns>
    protected override void Init() {
        base.Init();
        CanMove = true;
    }

    protected void RotateTorwardsTarget() {
        Vector3 forward;

        if (targetCell)
            forward = (targetCell.Position - transform.position).normalized;
        else
            forward = Vector3.ProjectOnPlane(previousDirection, Vector3.up);

            if (forward != Vector3.zero) {
                Quaternion toRot = Quaternion.LookRotation(forward, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, rotationSpeed * Time.deltaTime);

                if (forward != previousDirection) {
                    previousDirection = forward;
                    OnDirectionChanged();
                }
            }

    }

    protected abstract void OnDirectionChanged();
    protected abstract void OnCellChanged();

}
