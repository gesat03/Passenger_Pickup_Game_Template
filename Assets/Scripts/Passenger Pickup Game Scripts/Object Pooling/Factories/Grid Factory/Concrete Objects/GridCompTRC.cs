using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    // Grid Component Top Right Corner
    public class GridCompTRC : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Grid Objects/Grid Top Right Corner";
        }
        public int Size()
        {
            return 1;
        }
    }
}
