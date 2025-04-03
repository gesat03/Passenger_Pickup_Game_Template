using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class TransporterComp : IPoolObject
    {
        public string Root()
        {
            return "Prefabs/Wagon Objects/Transporter";
        }
        public int Size()
        {
            return 10;
        }
    }
}
