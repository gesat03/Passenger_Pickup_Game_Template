using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class WagonCompFact : BaseObjectFactory
    {
        private IPoolObject _wagonComponent;

        public WagonCompFact(IPoolObject wagonComponent, GameObject tranporter)
        {
            _wagonComponent = wagonComponent;

            ObjContainer = tranporter;

            CreateObjPool();
        }

        private void CreateObjPool()
        {
            ObjPool = new ObjectPool(_wagonComponent.Size(), _wagonComponent.Root(), ObjContainer);
        }
    }
}
