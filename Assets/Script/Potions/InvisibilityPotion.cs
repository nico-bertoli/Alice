using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityPotion : MonoBehaviour
{
    [SerializeField] float invisibilityDuration;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.GetComponent<Player>().ActivateInvisibility(invisibilityDuration);
                Destroy(gameObject);
            }
        }
}
