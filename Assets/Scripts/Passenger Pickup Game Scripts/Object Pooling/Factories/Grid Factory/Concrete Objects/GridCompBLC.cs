using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    // Grid Component Bottom Left Corner
    public class GridCompBLC : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Grid Objects/Grid Bottom Left Corner";
        }
        public int Size()
        {
            return 1;
        }
    }
}
