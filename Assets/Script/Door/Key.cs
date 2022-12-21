using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] int id;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            Door.KeyCollected(id);
            Win.WinKeyCollected(id);

            GameController.Instance.DoorisOpen = false;
            GameController.Instance.KeyisTaken = true;

            GameController.Instance.WinDoorisOpen = false;
            GameController.Instance.WinKeyisTaken = true;

            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
