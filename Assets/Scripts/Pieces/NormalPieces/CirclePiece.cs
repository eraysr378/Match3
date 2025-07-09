using Managers;
using Misc;
using UnityEngine;

namespace Pieces.NormalPieces
{
    public class CirclePiece : NormalPiece
    {
        protected override void PlayParticleEffect()
        {
            var particle = EventManager.RequestParticleSpawn?.Invoke(ParticleType.CircleExplosion,transform.position);
            particle?.SetParticleColor(GetColor());
            particle?.Play();
        }
    }
}