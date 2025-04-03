using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

namespace PPG
{
    [System.Serializable]
    public class CellData
    {
        public int Cell_ID;
        public Vector2 CellPosition;
        public bool IsOccupied = false;
    }
}
