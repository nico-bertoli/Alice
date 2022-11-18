using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : Singleton<WorldGrid> {
    private int nCols = 0;
    private int nRows = 0;
    private WorldCell[,] cells;

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
}
