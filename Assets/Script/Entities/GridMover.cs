using UnityEngine;

/// <summary>
/// Object that can move on world grid
/// </summary>
public abstract class GridMover : GridObject
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] private float rotationSpeed;

    public bool CanMove { get; set; } = false;
    public bool CanRotate { get; set; } = true;
    protected WorldCell targetCell;
    private WorldCell previousCell;
    protected Vector3 previousDirection;

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
            if (Vector3.Distance(transform.position, targetCell.Position) < 0.1f) {
                transform.position = targetCell.Position;
                currentCell = targetCell;
                targetCell = null;
                CanRotate = true;
            }
            if (previousCell != WorldGrid.Instance.GetCellAtPos(transform.position)) {
                if(previousCell)previousCell.CurrentObject = null;
                previousCell = CurrentCell;
                CurrentCell.CurrentObject = gameObject;
                OnCellChanged();
            }
        }
    }

    public void makeMoveTorwardsDirection(Vector2 _dir) {
        if (targetCell == null) {

            WorldCell target = WorldGrid.Instance.GetAdjacentCell(currentCell, _dir);
            if (target != null) {

                GameObject targetObj = target.CurrentObject;

                //if (targetObj != null) Debug.Log(targetObj.tag);

                if (targetObj != null && targetObj.tag == "Win")
                {
                    if (targetObj.GetComponent<Win>().TryOpenDor())
                    {
                        GameController.Instance.WinDoorisOpen = true;
                        GameController.Instance.WinKeyisTaken = false;
                        GameController.Instance.GameWon();
                    }
                }

                if (targetObj != null && targetObj.tag == "Dor")
                {
                    targetObj.GetComponent<Door>().TryOpenDor();
                    GameController.Instance.DoorisOpen = true;
                    GameController.Instance.KeyisTaken = false;
                }

                if (targetObj != null && targetObj.tag == "PushableBlock")
                    targetObj.GetComponent<PushableBlock>().Push(_dir);

                if (targetObj == null)
                    targetCell = WorldGrid.Instance.GetAdjacentCell(currentCell, _dir);
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
        if (CanRotate) {
            Vector3 forward;

            if (targetCell)
                forward = (targetCell.Position - transform.position).normalized;
            else
                forward = Vector3.ProjectOnPlane(previousDirection, Vector3.up);

            if (forward != Vector3.zero) {
                RotateAsVector(forward);
                if (forward != previousDirection) {
                    previousDirection = forward;
                    OnDirectionChanged();
                }
            }
        }
    }

    protected void RotateAwayFromTarget() {
        Vector3 forward;

        if (targetCell)
            forward = (transform.position - targetCell.Position).normalized;
        else
            forward = Vector3.ProjectOnPlane(previousDirection, Vector3.up);

        if (forward != Vector3.zero) {
            RotateAsVector(forward);
            if (forward != previousDirection) {
                previousDirection = forward;
                OnDirectionChanged();
            }
        }
    }

    protected void RotateAsVector(Vector3 _dir) {
        Quaternion toRot = Quaternion.LookRotation(_dir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, rotationSpeed * Time.deltaTime);
    }

    protected abstract void OnDirectionChanged();
    protected abstract void OnCellChanged();

}
