using Managers;
using Misc;
using UnityEngine;

namespace Pieces.NormalPieces
{
    public class TrianglePiece : NormalPiece
    {
        protected override void PlayParticleEffect()
        {
            var particle = EventManager.OnParticleSpawnRequested?.Invoke(ParticleType.TriangleExplosion,transform,transform.position,Vector3.one);
            particle?.transform.SetParent(null);
        }
    }
}