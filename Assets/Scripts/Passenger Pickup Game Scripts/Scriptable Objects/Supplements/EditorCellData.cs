using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    [System.Serializable]
    public class EditorCellData
    {
        public int CellID;
        public int Value;

        public bool IsSelected;
        public bool IsClosed;
        public bool IsBorder;

        public EItemColor WagonColor = EItemColor.Default;
        public int[] PassengerValue;
        public EItemColor[] PassengerColor;


        public Color GetColor(EItemColor color)
        {
            switch (color)
            {
                case EItemColor.Red:
                    return UnityEngine.Color.red;
                case EItemColor.Green:
                    return UnityEngine.Color.green;
                case EItemColor.Blue:
                    return UnityEngine.Color.blue;
                case EItemColor.Yellow:
                    return UnityEngine.Color.yellow;
                case EItemColor.Purple:
                    return UnityEngine.Color.magenta;
                case EItemColor.Default:
                    return UnityEngine.Color.gray;
                default:
                    return UnityEngine.Color.gray;
            }
        }
    }
}
