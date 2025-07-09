using Factories;
using Managers;
using Projectiles;
using UnityEngine;

namespace Spawners
{
    public class RainbowProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private RainbowProjectileFactory factory;
        [SerializeField] private Transform projectileParent;

        public void OnEnable()
        {
            EventManager.RequestRainbowProjectileSpawn += SpawnProjectile;
            EventManager.ReturnRainbowProjectileToPool += ReturnToPool;
        }

    
        private void OnDisable()
        {
            EventManager.RequestRainbowProjectileSpawn -= SpawnProjectile;
            EventManager.ReturnRainbowProjectileToPool -= ReturnToPool;

        }

        private void Awake()
        {
            factory.Initialize(projectileParent);
        }

        private RainbowProjectile SpawnProjectile(Vector3 position)
        {
            RainbowProjectile projectile = factory.Get();
            projectile.transform.position = position;
            return projectile;
        }
        private void ReturnToPool(RainbowProjectile projectile)
        {
            factory.ReturnToPool(projectile);
        }

  
    }
}