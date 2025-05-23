using Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace Factories.BaseFactories
{
    public abstract class BasePoolableObjectFactory<T> : ScriptableObject where T : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private T prefab;
        private ObjectPool<T> _pool;
        
        public T Get()
        {
            // at first call pool will be created
            if (_pool == null )
            {
                // Debug.Log("Pool Created ");
                _pool = new ObjectPool<T>(createFunc: Create,
                    actionOnGet: OnGetFromPool);
            }

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
            T obj = Instantiate(prefab);
            return obj;
        }
        // Because this is a scriptable object, the pool will not be destroyed between scene changes
        public void ResetPool()
        {
            _pool = null;
        }
    }
}