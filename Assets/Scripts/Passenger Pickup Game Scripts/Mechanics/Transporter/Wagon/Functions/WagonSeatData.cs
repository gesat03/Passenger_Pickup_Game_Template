using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    [System.Serializable]
    public class WagonSeatData
    {
        public Vector3 SeatPosition;
        public bool IsOccupied;
        public GameObject SeatObject;
    }
}
