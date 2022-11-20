using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingIndicator : MonoBehaviour
{
    //[SerializeField] float speed = 5f;
    [SerializeField] GameObject model;
    //private WorldCell target;

    public void HardSet(WorldCell _cell) {
        if (_cell != null) {
            transform.position = _cell.Position;
            //target = _cell;
            model.SetActive(true);
        }
        else model.SetActive(false);
    }

    //private void Update() {
    //    if (target != null) {
    //        if (transform.position != target.Position) {
    //            transform.position = Vector3.MoveTowards(transform.position, target.Position, speed * Time.deltaTime);
    //            if (Vector3.Distance(transform.position, target.Position) < Mathf.Epsilon)
    //                transform.position = target.Position;
    //        }
    //    }
    //}

    //public void MoveTo(WorldCell _cell) {
    //    if (_cell != null) {
    //        if (!model.activeSelf) {
    //            transform.position = _cell.Position;
    //            model.SetActive(true);
    //        }
    //        target = _cell;
    //    }
    //    else model.SetActive(false);
    //}
}
