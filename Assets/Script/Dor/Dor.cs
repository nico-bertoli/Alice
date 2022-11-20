using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dor : GridObject
{
    [SerializeField] int id;

    private static List<int> keysCollected = new List<int>();

    public void KeyCollected(int _id) { keysCollected.Add(_id); }

}
