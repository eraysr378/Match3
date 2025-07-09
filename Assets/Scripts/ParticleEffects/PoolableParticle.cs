using System;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;

namespace ParticleEffects
{
    public class PoolableParticle : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private ParticleType particleType;
        private ParticleSystem _particleSystem;
        public ParticleType GetParticleType() => particleType;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
        private void OnDisable()
        {
            OnReturnToPool();
        }

        public void OnSpawn()
        {
        }
        public void Play()
        {
            _particleSystem.Play(true);
        }
        public void OnReturnToPool()
        {
            _particleSystem.Clear(true);
            EventManager.ReturnParticleToPool?.Invoke(this);
        }

        public void SetParticleColor(Color color)
        {
            var main = _particleSystem.main;
            main.startColor = color;
        }
    }
}