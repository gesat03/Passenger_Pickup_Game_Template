using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PPG
{
    public class GridCompFact : BaseObjectFactory
    {
        private IPoolObject _gridComponent;

        public GridCompFact(IPoolObject gridComponent)
        {
            _gridComponent = gridComponent;

            CreateObjContainer();

            CreateObjPool();
        }

        private void CreateObjContainer()
        {
            ObjContainer = new GameObject();
            ObjContainer.transform.position = new Vector3(0, 0, 0);
            ObjContainer.name = _gridComponent.GetType().Name;
        }

        public GameObject GetGameObject()
        {
            return ObjContainer;
        }

        private void CreateObjPool()
        {
            ObjPool = new ObjectPool(_gridComponent.Size(), _gridComponent.Root(), ObjContainer);
        }
    }
}
