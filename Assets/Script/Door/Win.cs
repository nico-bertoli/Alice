using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : GridObject
{
    [SerializeField] int id;

    private static List<int> keysCollected = new List<int>();

    public static void KeyCollected(int _id) { keysCollected.Add(_id); }

    public void TryOpenDor()
    {
        if (keysCollected.Contains(id))
        {
            keysCollected.Remove(id);
            currentCell.CurrentObject = null;
            Destroy(gameObject);
            /// <summary>
            /// testing the player has won case
            /// </summary>             
            GameController.Instance.GameWon();
        }
    }
}
