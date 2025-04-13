using Interfaces;
using UnityEngine;

namespace ParticleEffects
{
    public class PoolableParticle : MonoBehaviour,IPoolableObject
    {
        [SerializeField] private ParticleType particleType;
        [SerializeField] private ParticleSystem particle;
        public ParticleType GetParticleType() => particleType;
        public void OnSpawn()
        {
       }

        public void OnReturnToPool()
        {
        }
    }
}