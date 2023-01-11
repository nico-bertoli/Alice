using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{

    [SerializeField] Vector3 target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().enabled = false;
            other.GetComponent<Player>().transform.position = target;
            other.GetComponent<Player>().enabled = true;
        }
    }

}
