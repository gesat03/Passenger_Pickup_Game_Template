using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    // Grid Component Straight Bottom
    public class GridCompSB : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Grid Objects/Grid Straight Bottom";
        }
        public int Size()
        {
            return 10;
        }
    }
}
