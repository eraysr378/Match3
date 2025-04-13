using Managers;
using Misc;
using UnityEngine;

namespace Pieces.NormalPieces
{
    public class SquarePiece : NormalPiece
    {
        protected override void PlayParticleEffect()
        {
            var particle = EventManager.OnParticleSpawnRequested?.Invoke(ParticleType.SquareExplosion, transform,
                transform.position, Vector3.one);
            particle?.transform.SetParent(null);
        }
    }
}