using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    //public enum eDirections {UP,RIGHT,BOTTOM,LEFT}

    /// <summary>
    /// Tells you if grid has been completely initialized
    /// </summary>
    public bool Initialized { get { return cells != null; } }
    /// <summary>
    /// Called after grid generation is completed
    /// </summary>
    public Action OnGridGenerationCompleted;
    /// <summary>
    /// the current scene buildindex
    /// </summary>
    int sceneBuildIndex;

    /// <summary>
    /// returns adjacent cell in given direction (null if not existing)
    /// </summary>
    /// <param name="_cell"></param>
    /// <param name="_dir"></param>
    /// <returns></returns>
    public WorldCell GetAdjacentCell(WorldCell _cell, Vector2 _dir) {

        WorldCell ris = null;
        try {

            //ris = cells[_cell.M - (int)Mathf.Ceil(_dir.y), _cell.N + (int)Mathf.Ceil(_dir.x)];
            ris = cells[_cell.M - Mathf.RoundToInt(_dir.y), _cell.N + Mathf.RoundToInt(_dir.x)];
        }
        // if the cell in given direction doesn't exist, null is returned
        catch {
            return null;
        }
        
        if (ris != null && Mathf.Abs(ris.Height - _cell.Height) > 1) ris = null;

        return ris;  
    }

    private void Start() {
        StartCoroutine(CompleteGridGeneration());
        /// <summary>
        /// setting global variables values at start of the level
        /// </summary>
        sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        GameController.Instance.SceneBuildIndex = sceneBuildIndex;
        GameController.Instance.IsOver = false;
        InputManager.Instance.IsPaused = false;
        GameController.Instance.PlayerWon = false;
    }

    /// <summary>
    /// Waits untill all cells have registered, and completes grid generation calling callback method
    /// </summary>
    /// <returns></returns>
    private IEnumerator CompleteGridGeneration() {
        do {
            yield return null;
        } while (WorldCell.registeredCells != WorldCell.numCells);

        //Debug.Log("matrix generation completed");
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
        //Debug.Log("Created matrix of size: "+nRows + "," + nCols);
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

    public Vector2 ConvertToVectorTwo(Vector3 _vector) {
        return new Vector2(_vector.z, -_vector.x);
    }
}
