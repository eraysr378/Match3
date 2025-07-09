using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace Factories.BaseFactories
{
    public abstract class BasePoolableObjectFactory<T> : ScriptableObject where T : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private int initialPoolSize;
        [SerializeField] private T prefab;
        private ObjectPool<T> _pool;
        private Transform _parent;
        public void Initialize(Transform parent)
        {
            _parent = parent;
            _pool = new ObjectPool<T>(createFunc: Create, actionOnGet: OnGetFromPool);
            var objList = new List<T>();
            for (int i = 0; i < initialPoolSize; i++)
            {
                var obj = _pool.Get();
                objList.Add(obj);
            }
            for (int i = 0; i < initialPoolSize; i++)
            {
                ReturnToPool(objList[i]);
            }
        }
        public T Get()
        {
            // at first call pool will be created
            // if (_pool == null )
            // {
            //     // Debug.Log("Pool Created ");
            //     _pool = new ObjectPool<T>(createFunc: Create,
            //         actionOnGet: OnGetFromPool);
            // }
            return _pool.Get();
        }

        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Release(obj);
        }

        private void OnGetFromPool(T obj)
        {
            obj.gameObject.SetActive(true);
            obj.OnSpawn();
        }

        private T Create()
        {
            T obj = Instantiate(prefab,_parent);
            return obj;
        }
        // Because this is a scriptable object, the pool will not be destroyed between scene changes
        public void ResetPool()
        {
            _pool = null;
        }
    }
}