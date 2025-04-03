using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;

namespace PPG
{
    [System.Serializable]
    public class GridMap
    {
        private GameObject[,] _cells;
        private CellData[,] _cellData;
        private int _rows;
        private int _columns;
        private float CellSize;

        public GridMap(int rows, int columns, float cellSize)
        {
            _rows = rows;
            _columns = columns;
            CellSize = cellSize;

            _cells = new GameObject[rows, columns];
            IdentifyCellData();
        }

        public void IdentifyCellData() //Identify the cell data
        {
            _cellData = new CellData[_rows, _columns];

            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _columns; y++)
                {
                    _cellData[x, y] = new CellData();
                }
            }
        }

        public float GetCellSize()
        {
            return CellSize;
        }

        public void AddCell(int rows, int columns, GameObject cell)
        {
            _cells[rows, columns] = cell;
        }

        public void SetCellData(int rows, int columns, Vector2 position) 
        {
            float y_position = _cells[rows, columns].transform.position.y;

            _cells[rows, columns].transform.position = new Vector3(position.x, y_position, position.y);

            _cellData[rows, columns].CellPosition = position;

            _cellData[rows, columns].Cell_ID = rows * _columns + columns + 1;

            Debug.Log("Cell ID: " + _cellData[rows, columns].Cell_ID + " - Cell Pos: " + _cellData[rows, columns].CellPosition);
        }

        public void SetCellOccupation(int cellID, bool isOccupied)
        {
            if (GetCellDataWithID(cellID) != null)
            {
                GetCellDataWithID(cellID).IsOccupied = isOccupied;
            }
        }

        public bool IsCellOccupied(int cellID)
        {
            if (GetCellDataWithID(cellID) != null)
            {
                return GetCellDataWithID(cellID).IsOccupied;
            }
            return false;
        }

        public Vector2 GetCellPositionWithID(int cellID)
        {
            if (GetCellDataWithID(cellID) != null)
            {
                return GetCellDataWithID(cellID).CellPosition;
            }
            Debug.Log("Cell position not found!");
            return Vector2.zero;
        }

        public CellData GetCellDataWithID(int cellID)
        {
            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _columns; y++)
                {
                    if (_cellData[x, y].Cell_ID == cellID)
                    {
                        return _cellData[x, y];
                    }
                }
            }
            Debug.Log("Cell ID not found!");
            return null;
        }

        public void ResetCell(int rows, int columns)
        {
            _cells = new GameObject[rows, columns];
        }

        public void DebugCells()
        {
            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _columns; y++)
                {
                    Debug.Log("Cell ID: " + _cellData[x, y].Cell_ID + " Is occupied: " + _cellData[x, y].IsOccupied);
                }
            }
        }
    }
}
