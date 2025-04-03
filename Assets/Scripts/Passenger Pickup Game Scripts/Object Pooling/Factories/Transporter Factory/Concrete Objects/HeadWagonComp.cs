using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class HeadWagonComp : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Wagon Objects/Wagon Head or Tail";
        }
        public int Size()
        {
            return 2;
        }
    }

}
