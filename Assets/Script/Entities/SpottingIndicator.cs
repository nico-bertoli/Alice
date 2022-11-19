using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingIndicator : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    private WorldCell target;
    private void Start() {
        //WorldGrid.Instance.OnGridGenerationCompleted += () => {
        //    target = WorldGrid.Instance.GetCellAtPos(transform.position);
        //};
    }

    private void Update() {
        if (target != null) {
            if (transform.position != target.Position) {
                transform.position = Vector3.MoveTowards(transform.position, target.Position, speed * Time.deltaTime);
                Debug.Log("moving torwards");
                if (Vector3.Distance(transform.position, target.Position) < Mathf.Epsilon)
                    transform.position = target.Position;
            }
        }
    }
    public void MoveTo(WorldCell _cell) {
        if (_cell != null) {
            if (!gameObject.activeSelf) {

                transform.position = _cell.Position;
                gameObject.SetActive(true);
            }
            target = _cell;
        }
        else gameObject.SetActive(false);
    }
    public void HardSet(WorldCell _cell) {
        if (_cell != null) {
            transform.position = _cell.Position;
            target = _cell;
        }
        else gameObject.SetActive(false);
    }
}
