using UnityEngine;

/// <summary>
/// Object that can move on world grid
/// </summary>
public abstract class GridMover : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] private float rotationSpeed;

    public WorldCell CurrentCell { get; private set; }
    protected WorldCell targetCell;

    private WorldCell previousCell;
    private Vector3 previousForward;

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
            CurrentCell = WorldGrid.Instance.GetCellAtPos(transform.position);
            if(previousCell != CurrentCell) {
                OnCellChanged();
                previousCell = CurrentCell;
            }
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
        CurrentCell = WorldGrid.Instance.GetCellAtPos(transform.position);
        transform.position = CurrentCell.Position;
    }

    private void RotateTorwardsTarget() {
        if (targetCell) {
            Vector3 forward = (targetCell.Position - transform.position).normalized;
            if (forward != Vector3.zero) {

                if(forward != previousForward) {
                    OnDirectionChanged();
                    previousForward = forward;
                }

                Quaternion toRot = Quaternion.LookRotation(forward, transform.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, rotationSpeed * Time.deltaTime);
            }
        }
    }

    protected abstract void OnDirectionChanged();
    protected abstract void OnCellChanged();

}
