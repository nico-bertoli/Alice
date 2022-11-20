using UnityEngine;

/// <summary>
/// Object that can move on world grid
/// </summary>
public abstract class GridMover : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] private float rotationSpeed;

    public bool CanMove { get; set; } = false;
    public WorldCell CurrentCell { get; private set; }
    protected WorldCell targetCell;

    private WorldCell previousCell;
    private Vector3 previousDirection;

    protected virtual void Start() {
        WorldGrid.Instance.OnGridGenerationCompleted += InitPosition;
    }

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
            CurrentCell = WorldGrid.Instance.GetCellAtPos(transform.position);
            if(Vector3.Distance(transform.position, targetCell.Position) < Mathf.Epsilon) {
                transform.position = targetCell.Position;
                targetCell = null;
            }
            if (previousCell != CurrentCell) {
                previousCell = CurrentCell;
                OnCellChanged();
            }
        }
    }

    /// <summary>
    /// Setup object position on starting cell
    /// </summary>
    /// <returns></returns>
    private void InitPosition() {
        CurrentCell = WorldGrid.Instance.GetCellAtPos(transform.position);
        transform.position = CurrentCell.Position;
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
