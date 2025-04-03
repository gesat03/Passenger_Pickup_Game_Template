using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class ObjectPool
    {
        private GameObject _prefab;
        private GameObject _prefabContainer;

        public List<GameObject> _poolList;

        public ObjectPool(int initialSize, string prefabPath, GameObject container) // Create a pool of objects
        {
            _prefabContainer = container;

            _poolList = new List<GameObject>();

            _prefab = Resources.Load<GameObject>(prefabPath);

            if (_prefab == null)
            {
                Debug.LogWarning("Prefab doesn't load!!!");
                return;
            }

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = GameObject.Instantiate(_prefab, _prefabContainer.transform);

                obj.SetActive(false);

                _poolList.Add(obj);
            }
        }

        public void SetExternalParent(GameObject parent) // Set the parent of the pool objects with an external parent
        {
            _prefabContainer = parent;

            foreach (var obj in _poolList)
            {
                obj.transform.SetParent(_prefabContainer.transform);
            }
        }

        public GameObject GetObjectFromPool() 
        {
            foreach (GameObject obj in _poolList)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            GameObject newObj = GameObject.Instantiate(_prefab, _prefabContainer.transform);

            _poolList.Add(newObj);

            return newObj;
        }

        public void ReleaseObjectToPoll(GameObject obj)
        {
            if (_poolList.Contains(obj))
            {
                obj.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Object is not part of this pool.");
            }
        }

        public void ReleaseAllObjectToPoll()
        {
            foreach (var obj in _poolList)
            {
                obj.SetActive(false);
            }
        }
    }
}
