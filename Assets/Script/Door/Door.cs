using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : GridObject
{
    [SerializeField] int id;

    private static List<int> keysCollected = new List<int>();

    public static void KeyCollected(int _id) { keysCollected.Add(_id); }

    public void TryOpenDor() {
        if (keysCollected.Contains(id)) {
            keysCollected.Remove(id);
            currentCell.CurrentObject = null;
            //Destroy(gameObject);
            gameObject.SetActive(false);
            //GameController.Instance.DoorisOpen = true;
            Debug.Log("soor is open true");

        }
    }

}
