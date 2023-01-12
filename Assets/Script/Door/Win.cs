using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Win : GridObject
{
    [SerializeField] int id;

    private static List<int> winKeysCollected = new List<int>();

    public static void WinKeyCollected(int _id) { winKeysCollected.Add(_id); }

    public bool TryOpenDor()
    {
        bool returnvalue = false;
        if (winKeysCollected.Contains(id))
        {
            winKeysCollected.Remove(id);
            currentCell.CurrentObject = null;
            //Destroy(gameObject);
            gameObject.SetActive(false);
            //GameController.Instance.GameWon();
            Debug.Log("swinoor is open true");
            returnvalue = true;
        }
        return returnvalue;
    }
}
