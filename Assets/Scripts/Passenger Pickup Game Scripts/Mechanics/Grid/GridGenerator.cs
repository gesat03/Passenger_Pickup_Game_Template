using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;

namespace PPG
{
    public class GridGenerator : MonoBehaviour
    {
        public GridMap Map;

        public int Rows = 5;
        public int Columns = 5;
        private float CellSize = 1f;

        public GameObject GridContainer;
        public GameObject MainCamera;

        private GridCompFact _gridCompFactTLC;  // Top Left Corner Component Factory
        private GridCompFact _gridCompFactTRC;  // Top Right Corner Component Factory
        private GridCompFact _gridCompFactBLC;  // Bottom Left Corner Component Factory
        private GridCompFact _gridCompFactBRC;  // Bottom Right Corner Component Factory
        private GridCompFact _gridCompFactS;    // Straight Component Factory
        private GridCompFact _gridCompFactST;   // Straight Top Component Factory
        private GridCompFact _gridCompFactSB;   // Straight Bottom Component Factory
        private GridCompFact _gridCompFactSR;   // Straight Left Component Factory
        private GridCompFact _gridCompFactSL;   // Straight Left Component Factory

        //private void Start()
        //{
        //    Initialization();
        //}

        public void Initialization() // Initialize the factory objects
        {
            _gridCompFactTLC = new GridCompFact(new GridCompTLC());
            _gridCompFactTLC.GetGameObject().transform.SetParent(GridContainer.transform);
            _gridCompFactTRC = new GridCompFact(new GridCompTRC());
            _gridCompFactTRC.GetGameObject().transform.SetParent(GridContainer.transform);
            _gridCompFactBLC = new GridCompFact(new GridCompBLC());
            _gridCompFactBLC.GetGameObject().transform.SetParent(GridContainer.transform);
            _gridCompFactBRC = new GridCompFact(new GridCompBRC());
            _gridCompFactBRC.GetGameObject().transform.SetParent(GridContainer.transform);
            _gridCompFactS = new GridCompFact(new GridCompS());
            _gridCompFactS.GetGameObject().transform.SetParent(GridContainer.transform);
            _gridCompFactST = new GridCompFact(new GridCompST());
            _gridCompFactST.GetGameObject().transform.SetParent(GridContainer.transform);
            _gridCompFactSB = new GridCompFact(new GridCompSB());
            _gridCompFactSB.GetGameObject().transform.SetParent(GridContainer.transform);
            _gridCompFactSR = new GridCompFact(new GridCompSR());
            _gridCompFactSR.GetGameObject().transform.SetParent(GridContainer.transform);
            _gridCompFactSL = new GridCompFact(new GridCompSL());
            _gridCompFactSL.GetGameObject().transform.SetParent(GridContainer.transform);
        }


        [ContextMenu("CreateGrid")]
        public void CreateGridMap(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            // Initialize the grid map
            Map = new GridMap(rows, columns, CellSize);

            // Reset the grid container position
            GridContainer.transform.position = Vector3.zero;
            MainCamera.transform.position = new Vector3(0, 30, -23.16f);

            // Creating cells starting from the top left corner of the grid
            Vector2 startPosition = new Vector2(-(rows - 1) * CellSize / 2, (columns - 1) * CellSize / 2);

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    // Calculating the world coordinates of the cell
                    Vector2 cellPosition = startPosition + new Vector2(x * CellSize, -y * CellSize);

                    ArrangeComponents(y, x, rows, columns, cellPosition); 

                    //Debug.Log($"Cell ({x - (Columns / 2)}, {y - (Rows / 2)}) location: {cellPosition}");
                }
            }

            // Set the camera position to the center of the grid
            MainCamera.transform.position = new Vector3(
                MainCamera.transform.position.x - (Map.GetCellPositionWithID(1).x - Map.GetCellPositionWithID(rows * columns).y),
                MainCamera.transform.position.y,
                MainCamera.transform.position.z - (Map.GetCellPositionWithID(1).y - Map.GetCellPositionWithID(rows * columns).x));
        }

        private void ArrangeComponents(int row, int column, int rows, int columns, Vector2 cellPosition) 
        {
            if (row == 0) /// Set the top row components
            {
                if (column == 0) // Set the top left corner component
                {
                    Map.AddCell(row, column, CreateGridComponent(EGridComponentOrientation.Top_Left_Corner));
                    Map.SetCellData(row, column, cellPosition);
                    return;
                }
                else if (column != columns - 1) // Set the top straight component
                {
                    Map.AddCell(row, column, CreateGridComponent(EGridComponentOrientation.Straight_Top));
                    Map.SetCellData(row, column, cellPosition);
                    return;
                }
            }
            if (column == columns - 1) /// Set the right column components
            {
                if (row == 0) // Set the top right corner component
                {
                    Map.AddCell(row, column, CreateGridComponent(EGridComponentOrientation.Top_Right_Corner));
                    Map.SetCellData(row, column, cellPosition);
                    return;
                }
                else if (row != rows - 1) // Set the right straight component
                {
                    Map.AddCell(row, column, CreateGridComponent(EGridComponentOrientation.Straight_Right));
                    Map.SetCellData(row, column, cellPosition);
                    return;
                }
            }
            if (row == rows - 1) /// Set the bottom row components
            {
                if (column == columns - 1) // Set the bottom right corner component
                {
                    Map.AddCell(row, column, CreateGridComponent(EGridComponentOrientation.Bottom_Right_Corner));
                    Map.SetCellData(row, column, cellPosition);
                    return;
                }
                else if (column != 0) // Set the bottom straight component
                {
                    Map.AddCell(row, column, CreateGridComponent(EGridComponentOrientation.Straight_Bottom));
                    Map.SetCellData(row, column, cellPosition);
                    return;
                }
            }
            if (column == 0) /// Set the left column components
            {
                if (row == rows - 1) // Set the bottom left corner component
                {
                    Map.AddCell(row, column, CreateGridComponent(EGridComponentOrientation.Bottom_Left_Corner));
                    Map.SetCellData(row, column, cellPosition);
                    return;
                }
                else if (row != 0) // Set the left straight component
                {
                    Map.AddCell(row, column, CreateGridComponent(EGridComponentOrientation.Straight_Left));
                    Map.SetCellData(row, column, cellPosition);
                    return;
                }
            }
            else /// Set the straight components
            {
                Map.AddCell(row, column, CreateGridComponent(EGridComponentOrientation.Straight));
                Map.SetCellData(row, column, cellPosition);
                return;
            }
        }

        public GameObject CreateGridComponent(EGridComponentOrientation orientation)
        {
            switch (orientation)
            {
                case EGridComponentOrientation.Top_Left_Corner:
                    return _gridCompFactTLC.CreateObj();
                case EGridComponentOrientation.Top_Right_Corner:
                    return _gridCompFactTRC.CreateObj();
                case EGridComponentOrientation.Bottom_Left_Corner:
                    return _gridCompFactBLC.CreateObj();
                case EGridComponentOrientation.Bottom_Right_Corner:
                    return _gridCompFactBRC.CreateObj();
                case EGridComponentOrientation.Straight_Top:
                    return _gridCompFactST.CreateObj();
                case EGridComponentOrientation.Straight_Bottom:
                    return _gridCompFactSB.CreateObj();
                case EGridComponentOrientation.Straight_Left:
                    return _gridCompFactSL.CreateObj();
                case EGridComponentOrientation.Straight_Right:
                    return _gridCompFactSR.CreateObj();
                case EGridComponentOrientation.Straight:
                    return _gridCompFactS.CreateObj();
                default:
                    return _gridCompFactS.CreateObj();
            }
        }

        public void ReleaseComponent(EGridComponentOrientation orientation, GameObject component)
        {
            switch (orientation)
            {
                case EGridComponentOrientation.Top_Left_Corner:
                    _gridCompFactTLC.ReleaseObj(component);
                    break;
                case EGridComponentOrientation.Top_Right_Corner:
                    _gridCompFactTRC.ReleaseObj(component);
                    break;
                case EGridComponentOrientation.Bottom_Left_Corner:
                    _gridCompFactBLC.ReleaseObj(component);
                    break;
                case EGridComponentOrientation.Bottom_Right_Corner:
                    _gridCompFactBRC.ReleaseObj(component);
                    break;
                case EGridComponentOrientation.Straight_Top:
                    _gridCompFactST.ReleaseObj(component);
                    break;
                case EGridComponentOrientation.Straight_Bottom:
                    _gridCompFactSB.ReleaseObj(component);
                    break;
                case EGridComponentOrientation.Straight_Left:
                    _gridCompFactSL.ReleaseObj(component);
                    break;
                case EGridComponentOrientation.Straight_Right:
                    _gridCompFactSR.ReleaseObj(component);
                    break;
                case EGridComponentOrientation.Straight:
                    _gridCompFactS.ReleaseObj(component);
                    break;
                default:
                    _gridCompFactS.ReleaseObj(component);
                    break;
            }
        }

        [ContextMenu("DebugCells")]
        private void DebugCells()
        {
            Map.DebugCells();
        }

        [ContextMenu("ReleaseAllComponent")]
        public void ReleaseAllGrids()
        {
            _gridCompFactTLC.ReleaseAllObj();
            _gridCompFactTRC.ReleaseAllObj();
            _gridCompFactBLC.ReleaseAllObj();
            _gridCompFactBRC.ReleaseAllObj();
            _gridCompFactS.ReleaseAllObj();
            _gridCompFactST.ReleaseAllObj();
            _gridCompFactSB.ReleaseAllObj();
            _gridCompFactSR.ReleaseAllObj();
            _gridCompFactSL.ReleaseAllObj();
        }
    }
}
