using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] int id;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            Door.KeyCollected(id);
            Win.KeyCollected(id);
            FindObjectOfType<GameController>().DoorisOpen = false;
            //Destroy(gameObject);
            gameObject.SetActive(false);
            FindObjectOfType<GameController>().KeyisTaken = true;
        }
    }
}
