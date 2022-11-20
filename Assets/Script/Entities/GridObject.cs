using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected WorldCell currentCell;
    public WorldCell CurrentCell { get { return currentCell; } private set { currentCell = value; } }

    protected virtual void Start() {
        WorldGrid.Instance.OnGridGenerationCompleted += Init;
    }

    /// <summary>
    /// Setup object position on starting cell
    /// </summary>
    /// <returns></returns>
    protected virtual void Init() {
        currentCell = WorldGrid.Instance.GetCellAtPos(transform.position);
        transform.position = CurrentCell.Position;
        CurrentCell.Walkable = false;
    }

}
