using Managers;
using Misc;
using UnityEngine;

namespace Pieces.NormalPieces
{
    public class CirclePiece : NormalPiece
    {
        protected override void PlayParticleEffect()
        {
            var particle = EventManager.OnParticleSpawnRequested?.Invoke(ParticleType.CircleExplosion,transform,transform.position,Vector3.one);
            particle?.transform.SetParent(null);    
        }
    }
}