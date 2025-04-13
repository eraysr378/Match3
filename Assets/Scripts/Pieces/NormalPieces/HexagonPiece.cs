using Managers;
using Misc;
using UnityEngine;

namespace Pieces.NormalPieces
{
    public class HexagonPiece : NormalPiece
    {
        protected override void PlayParticleEffect()
        {
            var particle = EventManager.OnParticleSpawnRequested?.Invoke(ParticleType.HexagonExplosion, transform,
                transform.position, Vector3.one);
            particle?.transform.SetParent(null);
        }
    }
}