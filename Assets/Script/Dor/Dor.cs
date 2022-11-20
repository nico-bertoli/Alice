using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dor : MonoBehaviour
{
    [SerializeField] int id;

    private static List<int> keysCollected = new List<int>();

    public void KeyCollected(int _id) { keysCollected.Add(_id); }

}
