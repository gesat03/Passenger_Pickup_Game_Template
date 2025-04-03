using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class WagonData : MonoBehaviour
    {
        public int WagonPosID;
        public EWagonDirections WagonDirection;
        public EItemColor WagonColor;
        public bool IsHeadWagon;
        public bool IsTailWagon;
        public bool IsWagonLast;

        public void IsWagonHeaded(bool isHead)
        {
            if (isHead)
            {
                IsHeadWagon = true;
                IsTailWagon = false;
            }
            else
            {
                IsHeadWagon = false;
                IsTailWagon = true;

                IsWagonLast = true;
            }
        }
    }
}
