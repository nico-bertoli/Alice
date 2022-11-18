using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : Singleton<WorldGrid> {
    private int nCols = 0;
    private int nRows = 0;
    private WorldCell[,] cells;

    public enum eDirections { UP,RIGHT,BOTTOM,LEFT}
    public bool Initialized { get { return cells != null; } }

    public static eDirections VectorToDir(Vector2 _vec) {
        if(_vec == Vector2.left)return eDirections.LEFT;
        if(_vec == Vector2.right)return eDirections.RIGHT;
        if(_vec == Vector2.up)return eDirections.UP;
        else return eDirections.BOTTOM;
    }

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

        if(ris == null) Debug.Log("target cannot be reached");
        else Debug.Log("new target:" + ris.M + "," + ris.N + " can be reached");

        return ris;
            
    }


    public void RegisterCell(WorldCell _cell) {

        if (cells == null)
            CreateMatrix();

        if (cells[_cell.M, _cell.N] == null)
            cells[_cell.M, _cell.N] = _cell;
        else if ( cells[_cell.M, _cell.N].Height < _cell.Height) {
            Destroy(cells[_cell.M, _cell.N]);
            cells[_cell.M, _cell.N] = _cell;
        }
        else
            Destroy(_cell);
    }

    private void CreateMatrix() {
        nCols = WorldCell.maxN + 1;
        nRows = WorldCell.maxM + 1;
        cells = new WorldCell[nRows, nCols];
        Debug.Log("Created matrix of size: "+nRows + "," + nCols);
    }

    public WorldCell GetCellAtPos(Vector3 _pos) {
        int M = (int)_pos.x;
        int N = (int)_pos.z;
        return cells[M, N];
    }
}
