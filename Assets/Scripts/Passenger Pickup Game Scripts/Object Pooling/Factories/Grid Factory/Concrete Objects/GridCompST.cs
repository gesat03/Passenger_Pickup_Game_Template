using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    // Grid Component Straight Top
    public class GridCompST : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Grid Objects/Grid Straight Top";
        }
        public int Size()
        {
            return 10;
        }
    }
}
