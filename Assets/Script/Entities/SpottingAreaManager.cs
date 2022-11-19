using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingAreaManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject spottingIndicatorPref;

    private SpottingIndicator spottingIndicator;
    private WorldCell cell;
    private Player player;

    private void Awake() {
        GetComponent<MeshRenderer>().enabled = false;
        spottingIndicator = Instantiate(spottingIndicatorPref.GetComponent<SpottingIndicator>());
    }

    private void Start() {
        player = GameController.Instance.Player;
        WorldGrid.Instance.OnGridGenerationCompleted +=
            () => {
                cell = WorldGrid.Instance.GetCellAtPos(transform.position);
                if (cell != null)
                    spottingIndicator.transform.position = cell.Position;
            };
    }

    private void Update() {
        if (player && cell == player.CurrentCell)
            GameController.Instance.GameOver();
    }

    public void TraslateArea() {
        spottingIndicator.MoveTo(WorldGrid.Instance.GetCellAtPos(transform.position));
    }
    public void HardSetArea() {
        spottingIndicator.HardSet(WorldGrid.Instance.GetCellAtPos(transform.position));
    }
}









//public class SpottingAreaManager : MonoBehaviour {
//    [Header("References")]
//    [SerializeField] GameObject spottingAreaIndicatorPref;

//    private SpottingAreaIndicator spottingAreaIndicator;
//    private WorldCell cell;
//    private Player player;

//    private void Awake() {
//        GetComponent<MeshRenderer>().enabled = false;
//        spottingAreaIndicator = Instantiate(spottingAreaIndicatorPref.GetComponent<SpottingAreaIndicator>());
//    }

//    private void Start() {
//        player = GameController.Instance.Player;
//        WorldGrid.Instance.OnGridGenerationCompleted +=
//            () => {
//                cell = WorldGrid.Instance.GetCellAtPos(transform.position);
//                if (cell != null)
//                    spottingAreaIndicator.transform.position = cell.Position;
//            };
//    }

//    private void Update() {
//        if (cell == player.CurrentCell)
//            GameController.Instance.GameOver();
//    }

//    public void TraslateArea() {
//        spottingAreaIndicator.MoveTo(WorldGrid.Instance.GetCellAtPos(transform.position));
//    }
//    public void HardSetArea() {
//        spottingAreaIndicator.HardSet(WorldGrid.Instance.GetCellAtPos(transform.position));
//    }
//}



////----------------------



//using Cinemachine.Utility;
//using Newtonsoft.Json.Linq;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SpottingAreaIndicator : MonoBehaviour {
//    [SerializeField] float speed = 5f;

//    private WorldCell target;

//    private void Start() {
//        //WorldGrid.Instance.OnGridGenerationCompleted += () => {
//        //    target = WorldGrid.Instance.GetCellAtPos(transform.position);
//        //};
//    }

//    private void Update() {
//        if (target != null) {
//            if (transform.position != target.Position) {
//                transform.position = Vector3.MoveTowards(transform.position, target.Position, speed * Time.deltaTime);
//                Debug.Log("moving torwards");
//                if (Vector3.Distance(transform.position, target.Position) < Mathf.Epsilon)
//                    transform.position = target.Position;
//            }
//        }
//    }
//    public void MoveTo(WorldCell _cell) {
//        if (_cell != null) {
//            if (!gameObject.activeSelf) {
//                transform.position = target.Position;
//                gameObject.SetActive(true);
//            }
//            target = _cell;
//        }
//        else gameObject.SetActive(false);
//    }
//    public void HardSet(WorldCell _cell) {
//        if (_cell != null) {
//            transform.position = _cell.Position;
//            target = _cell;
//        }
//        else gameObject.SetActive(false);
//    }


//}

