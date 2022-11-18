using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : Singleton<WorldGrid> {
    private int nCols = 0;
    private int nRows = 0;
    private WorldCell[,] cells;

    public enum eDirections { UP,RIGHT,BOTTOM,LEFT}

    public static eDirections VectorToDir(Vector2 _vec) {
        if(_vec == Vector2.left)return eDirections.UP;
        if(_vec == Vector2.right)return eDirections.BOTTOM;
        if(_vec == Vector2.up)return eDirections.LEFT;
        else return eDirections.UP;
    }

    public WorldCell GetAdjacentCell(WorldCell _cell, eDirections _dir) {
        WorldCell ris = null;
        switch (_dir) {
            case eDirections.UP:
                ris = cells[_cell.M-1,_cell.N];
                break;
            case eDirections.BOTTOM:
                ris = cells[_cell.M+1, _cell.N];
                break;
            case eDirections.RIGHT:
                ris = cells[_cell.M, _cell.N+1];
                break;
            case eDirections.LEFT:
                ris = cells[_cell.M, _cell.N-1];
                break;
        }

        if (Mathf.Abs(ris.Height - _cell.Height) <= 1)
            return ris;
        else
            return null;
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
