using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class MidWagonComp : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Wagon Objects/Wagon Mid";
        }
        public int Size()
        {
            return 5;
        }
    }
}
