using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RolesManager;
public class Dress : MonoBehaviour
{
    [SerializeField] eRoles disguiseType;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Player>().SetDisguise(disguiseType);
            Destroy(gameObject);
        }
    }

}
