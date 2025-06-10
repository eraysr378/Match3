using Managers;
using Misc;
using ParticleEffects;
using UnityEngine;

namespace Pieces.NormalPieces
{
    public class SquarePiece : NormalPiece
    {
        protected override void PlayParticleEffect()
        {
            PoolableParticle particle = EventManager.OnParticleSpawnRequested?.Invoke(ParticleType.SquareExplosion,
                transform.position);
            particle?.SetParticleColor(GetColor());
            particle?.Play();
        }
    }
}