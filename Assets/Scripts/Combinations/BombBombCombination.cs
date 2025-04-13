using System.Collections.Generic;
using Cells;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;
using Utils;

namespace Combinations
{
    public class BombBombCombination : BaseCombination
    {
        [SerializeField] private int explosionRadius;
        private readonly CellDirtyTracker _cellDirtyTracker = new();

        protected override void ActivateCombination(int row, int col)
        {
            var cellsInRadius = GridManager.Instance.GetCellsInRadius(row, col, explosionRadius);
            PlayParticle();

            _cellDirtyTracker.Mark(cellsInRadius);

            List<IExplodable> explodables =
                GridManager.Instance.GetPiecesInRadius<IExplodable>(row, col, explosionRadius);

            foreach (var explodable in explodables)
            {
                explodable.TryExplode();
            }

            CompleteCombination();
        }

        protected override void CompleteCombination()
        {
            _cellDirtyTracker.ClearAll();
            base.CompleteCombination();
        }
        private void PlayParticle()
        {
            var particle = EventManager.OnParticleSpawnRequested?.Invoke(ParticleType.BombBombExplosion, transform,
                transform.position, Vector3.one);
            particle?.transform.SetParent(null);
        }
    }
}