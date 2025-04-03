using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class NPCObject : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/NPC/Stick Man";
        }
        public int Size()
        {
            return 50;
        }
    }
}
