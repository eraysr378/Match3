using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public class AudioSourcePool : MonoBehaviour
    {
        [SerializeField] private PooledAudioSource prefab;
        [SerializeField] private int poolDefaultCapacity = 20;
        [SerializeField] private int poolMaxSize = 50;
        [SerializeField] private int active;
        [SerializeField] private int inactive;
        [SerializeField] private int countall;
        private ObjectPool<PooledAudioSource> _pool;

        public void Initialize()
        {
            _pool = new ObjectPool<PooledAudioSource>(createFunc: Create,
                actionOnGet: OnGetFromPool, defaultCapacity: poolDefaultCapacity, maxSize: poolMaxSize);
            List<PooledAudioSource> prefillList = new();
            for (int i = 0; i < poolDefaultCapacity; i++)
            {
                var pooledAudioSource = _pool.Get();
                pooledAudioSource.Initialize(this);
                prefillList.Add(pooledAudioSource);
            }

            foreach (var prefill in prefillList)
            {
                ReturnToPool(prefill);
            }
        }

        // private void Update()
        // {
        //     active = _pool.CountActive;
        //     inactive = _pool.CountInactive;
        //     countall = _pool.CountAll;
        // }

        public void ReturnToPool(PooledAudioSource obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Release(obj);
        }

        private void OnGetFromPool(PooledAudioSource obj)
        {
            obj.gameObject.SetActive(true);
            obj.OnSpawn();
        }

        private PooledAudioSource GetFromPool()
        {
            return _pool.Get();
        }

        private PooledAudioSource Create()
        {
            var pooledAudioSource = Instantiate(prefab, transform);
            pooledAudioSource.Initialize(this);
            return pooledAudioSource;
        }

        public void PlayOneShot(AudioClip clip, float pitch = 1f, float volume = 1f)
        {
            var source = GetFromPool();
            source?.PlayOneShot(clip, pitch, volume);
        }
        // public PooledAudioSource PlayLoop(AudioClip clip, float pitch = 1f, float volume = 1f)
        // {
        //     var source = GetFromPool();
        //     source?.PlayLoop(clip, pitch, volume);
        //     return source;
        // }
    }
}