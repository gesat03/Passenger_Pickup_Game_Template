using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public abstract class BaseObjectFactory
    {
        public ObjectPool ObjPool;

        public GameObject ObjContainer;

        public virtual GameObject CreateObj()
        {
            return ObjPool.GetObjectFromPool();
        }

        public virtual void ReleaseObj(GameObject obj)
        {
            ObjPool.ReleaseObjectToPoll(obj);
        }

        public virtual void ReleaseAllObj()
        {
            ObjPool.ReleaseAllObjectToPoll();
        }

    }
}
