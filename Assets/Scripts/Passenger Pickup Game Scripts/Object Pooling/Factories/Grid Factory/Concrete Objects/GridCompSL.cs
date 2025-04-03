using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    // Grid Component Straight Left
    public class GridCompSL : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Grid Objects/Grid Straight Left";
        }
        public int Size()
        {
            return 10;
        }
    }
}
