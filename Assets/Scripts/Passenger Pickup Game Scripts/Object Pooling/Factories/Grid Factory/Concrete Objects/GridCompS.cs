using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    // Grid Component Straight
    public class GridCompS : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Grid Objects/Grid Straight";
        }
        public int Size()
        {
            return 40;
        }
    }
}
