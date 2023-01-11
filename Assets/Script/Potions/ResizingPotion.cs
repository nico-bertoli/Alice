using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizingPotion : MonoBehaviour
{
    [SerializeField] float resizingDuration;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.GetComponent<Player>().ActivateResizing(resizingDuration);
                Destroy(gameObject);
            }
        }
}
