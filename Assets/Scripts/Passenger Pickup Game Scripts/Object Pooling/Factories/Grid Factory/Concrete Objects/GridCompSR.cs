using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    // Grid Component Straight Right
    public class GridCompSR : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Grid Objects/Grid Straight Right";
        }
        public int Size()
        {
            return 10;
        }
    }
}
