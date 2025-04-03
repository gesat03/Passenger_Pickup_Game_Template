using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    // Grid Component Top Left Corner
    public class GridCompTLC : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Grid Objects/Grid Top Left Corner";
        }
        public int Size()
        {
            return 1;
        }
    }
}
