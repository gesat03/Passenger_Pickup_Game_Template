using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class PassageFactory : BaseObjectFactory
    {
        private IPoolObject _nPCObject;

        public PassageFactory(IPoolObject nPCObject, GameObject tranporter)
        {
            _nPCObject = nPCObject;

            ObjContainer = tranporter;

            CreateObjPool();
        }

        private void CreateObjPool()
        {
            ObjPool = new ObjectPool(_nPCObject.Size(), _nPCObject.Root(), ObjContainer);
        }
    }
}
