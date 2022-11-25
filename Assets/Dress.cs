using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dress : MonoBehaviour
{
    [SerializeField] Player.eDisguises disguiseType;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Player>().SetDisguise(disguiseType);
            Destroy(gameObject);
        }
    }

}
