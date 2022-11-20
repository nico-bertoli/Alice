using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : Singleton<WorldGrid> {

    /// <summary>
    /// number of columns
    /// </summary>
    private int nCols = 0;
    /// <summary>
    /// number of rows
    /// </summary>
    private int nRows = 0;
    /// <summary>
    /// matrix of cells
    /// </summary>
    private WorldCell[,] cells;
    /// <summary>
    /// Possible grid directions (diagonal not allowed)
    /// </summary>
    public enum eDirections { UP,RIGHT,BOTTOM,LEFT}

    /// <summary>
    /// Tells you if grid has been completely initialized
    /// </summary>
    public bool Initialized { get { return cells != null; } }
    /// <summary>
    /// Called after grid generation is completed
    /// </summary>
    public Action OnGridGenerationCompleted;
    
    /// <summary>
    /// Converts vector 2 to direction
    /// </summary>
    /// <param name="_vec"></param>
    /// <returns></returns>
    public static eDirections VectorToDir(Vector2 _vec) {
        if(_vec == Vector2.left)return eDirections.LEFT;
        if(_vec == Vector2.right)return eDirections.RIGHT;
        if(_vec == Vector2.up)return eDirections.UP;
        else return eDirections.BOTTOM;
    }

    /// <summary>
    /// returns adjacent cell in given direction (null if not existing)
    /// </summary>
    /// <param name="_cell"></param>
    /// <param name="_dir"></param>
    /// <returns></returns>
    public WorldCell GetAdjacentCell(WorldCell _cell, eDirections _dir) {
        WorldCell ris = null;
        switch (_dir) {
            case eDirections.UP:
                if(_cell.M != 0)ris = cells[_cell.M-1,_cell.N];
                break;
            case eDirections.BOTTOM:
                if(_cell.M != nRows-1) ris = cells[_cell.M+1, _cell.N];
                break;
            case eDirections.RIGHT:
                if(_cell.N != nCols-1) ris = cells[_cell.M, _cell.N+1];
                break;
            case eDirections.LEFT:
                if(_cell.N!= 0) ris = cells[_cell.M, _cell.N-1];
                break;
        }

        if (ris != null && Mathf.Abs(ris.Height - _cell.Height) > 1) ris = null;

        return ris;
            
    }

    private void Start() {
        StartCoroutine(CompleteGridGeneration());
    }

    /// <summary>
    /// Waits untill all cells have registered, and completes grid generation calling callback method
    /// </summary>
    /// <returns></returns>
    private IEnumerator CompleteGridGeneration() {
        do {
            yield return null;
        } while (WorldCell.registeredCells != WorldCell.numCells);

        Debug.Log("matrix generation completed");
        OnGridGenerationCompleted();
    }

//---------------------------------------- registering cells
public void RegisterCell(WorldCell _cell) {

        if (cells == null)
            CreateMatrix();

        tryGeneratingNotExistingCell(_cell);
        tryCoveringExistingCell(_cell);            
    }

    /// <summary>
    /// if cell is not present, inits it with given cell
    /// </summary>
    /// <param name="_cell"></param>
    private void tryGeneratingNotExistingCell(WorldCell _cell) {
        if (cells[_cell.M, _cell.N] == null) cells[_cell.M, _cell.N] = _cell;
    }
    /// <summary>
    /// if another cell is already present, keeps the higer one
    /// </summary>
    /// <param name="_cell"></param>
    private void tryCoveringExistingCell(WorldCell _cell) {

        if (cells[_cell.M, _cell.N] == _cell) return;

        if (cells[_cell.M, _cell.N].Height < _cell.Height) {
            Destroy(cells[_cell.M, _cell.N]);
            cells[_cell.M, _cell.N] = _cell;
        }
        else {
            Destroy(_cell);
        }
    }

    //---------------------------------------------

    private void CreateMatrix() {
        nCols = WorldCell.maxN + 1;
        nRows = WorldCell.maxM + 1;
        cells = new WorldCell[nRows, nCols];
        Debug.Log("Created matrix of size: "+nRows + "," + nCols);
    }

    /// <summary>
    /// Returns the cell realative to this object position
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public WorldCell GetCellAtPos(Vector3 _pos) {
        int M = Mathf.RoundToInt(_pos.x);
        int N = Mathf.RoundToInt(_pos.z);

        if (M >= 0 && N >= 0 && M < nRows && N < nCols)
            return cells[M, N];
        else
            return null;
    }
}
