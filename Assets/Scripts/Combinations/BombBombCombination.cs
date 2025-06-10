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
            _cellDirtyTracker.Mark(cellsInRadius);
            foreach (var cell in cellsInRadius)
            {
                cell.TriggerExplosion();
            }
            PlayParticle();
            EventManager.OnBigCameraShakeRequest?.Invoke();
            CompleteCombination();
        }

        protected override void CompleteCombination()
        {
            _cellDirtyTracker.ClearAll();
            base.CompleteCombination();
        }

        private void PlayParticle()
        {
            var particle = EventManager.OnParticleSpawnRequested?.Invoke(ParticleType.BombBombExplosion,
                transform.position);
            particle?.transform.SetParent(null);
            particle?.Play();
        }
    }
}