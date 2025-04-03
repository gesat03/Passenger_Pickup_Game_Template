using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    // Grid Component Bottom Right Corner
    public class GridCompBRC : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Grid Objects/Grid Bottom Right Corner";
        }
        public int Size()
        {
            return 1;
        }
    }
}
