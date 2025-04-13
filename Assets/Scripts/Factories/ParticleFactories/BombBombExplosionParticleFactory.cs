using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.ParticleFactories
{
    [CreateAssetMenu(fileName = "BombBombExplosionParticle",
        menuName = "Factories/Particle/BombBombExplosionParticle")]
    public class BombBombExplosionParticleFactory : BaseParticleFactory
    {
        public override bool CanCreateParticle(ParticleType particleType)
        {
            return particleType == ParticleType.BombBombExplosion;
        }
    }
}