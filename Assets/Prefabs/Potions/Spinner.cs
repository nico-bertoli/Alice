using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float spinSpeed = 0;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, spinSpeed, 0));
    }
}
