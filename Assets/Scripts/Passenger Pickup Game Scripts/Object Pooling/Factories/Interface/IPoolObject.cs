using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public interface IPoolObject
    {
        public string Root();
        public int Size();
    }
}
