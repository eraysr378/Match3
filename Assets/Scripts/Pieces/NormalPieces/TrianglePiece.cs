using Managers;
using Misc;
using UnityEngine;

namespace Pieces.NormalPieces
{
    public class TrianglePiece : NormalPiece
    {
        protected override void PlayParticleEffect()
        {
            var particle = EventManager.RequestParticleSpawn?.Invoke(ParticleType.TriangleExplosion,transform.position);
            particle?.SetParticleColor(GetColor());
            particle?.Play();
        }
    }
}