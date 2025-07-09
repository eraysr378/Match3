using Managers;
using Misc;
using UnityEngine;

namespace Pieces.NormalPieces
{
    public class HexagonPiece : NormalPiece
    {
        protected override void PlayParticleEffect()
        {
            var particle = EventManager.RequestParticleSpawn?.Invoke(ParticleType.HexagonExplosion,
                transform.position);
            particle?.SetParticleColor(GetColor());
            particle?.Play();
        }
    }
}