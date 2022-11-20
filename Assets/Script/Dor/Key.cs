using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] int id;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            Dor.KeyCollected(id);
            Destroy(gameObject);
        }
    }
}
