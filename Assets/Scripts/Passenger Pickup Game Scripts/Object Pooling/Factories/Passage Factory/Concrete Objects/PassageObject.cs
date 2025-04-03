using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class PassageObject : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Passage Objects/Passage";
        }
        public int Size()
        {
            return 10;
        }
    }
}
