using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace PPG
{
    [CreateAssetMenu(fileName = "NewLevelDataSO", menuName = "Grid System/Level Data SO")]
    public class LevelDataScriptableObject : ScriptableObject
    {
        public int gridSizeX;
        public int gridSizeY;
        public EditorCellData[] cells;

        public void Initialize(int sizeX, int sizeY)
        {
            gridSizeX = sizeX;
            gridSizeY = sizeY;
            cells = new EditorCellData[sizeX * sizeY];

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new EditorCellData { Value = 0 };
            }

            MarkCornersAndBorders();
        }

        private void MarkCornersAndBorders()
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int x = 0; x < gridSizeX; x++)
                {
                    int index = y * gridSizeX + x;

                    bool isCorner = (x == 0 && y == 0) ||
                                   (x == gridSizeX - 1 && y == 0) ||
                                   (x == 0 && y == gridSizeY - 1) ||
                                   (x == gridSizeX - 1 && y == gridSizeY - 1);

                    bool isBorder = (x == 0 || x == gridSizeX - 1 || y == 0 || y == gridSizeY - 1) && !isCorner;

                    cells[index].IsClosed = isCorner;
                    cells[index].IsBorder = isBorder;
                }
            }
        }

        public EditorCellData GetCell(int x, int y)
        {
            if (x < 0 || x >= gridSizeX || y < 0 || y >= gridSizeY) return null;
            return cells[y * gridSizeX + x];
        }

        public void SetWagonValue(int x, int y, int newValue, EItemColor color)
        {
            var cell = GetCell(x, y);
            if (cell == null) return;

            cell.Value = newValue;
            cell.IsSelected = (newValue != 0);
            cell.WagonColor = color;
        }

        public void SetPassengerValue(int x, int y, int[] newValueArray, EItemColor[] colorArray, int newValue)
        {
            var cell = GetCell(x, y);
            if (cell == null) return;

            cell.Value = newValue;
            cell.IsSelected = (newValue != 0);
            cell.PassengerValue = new int[newValue];
            cell.PassengerColor = new EItemColor[newValue];
            
            cell.PassengerValue = newValueArray;
            cell.PassengerColor = colorArray;
        }

    }
}
