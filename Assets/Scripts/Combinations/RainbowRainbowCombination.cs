using System.Collections;
using System.Collections.Generic;
using Cells;
using Interfaces;
using Managers;
using Misc;
using ParticleEffects;
using Pieces;
using UnityEngine;
using Utils;


namespace Combinations
{
    public class RainbowRainbowCombination : BaseCombination
    {
        private ParticleHandler _particleHandler;
        private int _explosionCount;
        private readonly CellDirtyTracker _dirtyTracker = new();

        public override void Awake()
        {
            base.Awake();
            _particleHandler = GetComponent<ParticleHandler>();
        }

        public override void OnSpawn()
        {
            visual.enabled = true;
        }

        protected override void ActivateCombination(int row, int col)
        {
            visual.enabled = false;
            _particleHandler.Play(ParticleType.Activation);
            DestroyAll(row, col);
        }

        private void DestroyAll(int row, int col)
        {
            List<Cell> allCells = GridManager.Instance.GetAllCells();
            _dirtyTracker.Mark(allCells);
            float delayPerDistance = 0.05f;
            _explosionCount = allCells.Count;
            foreach (var cell in allCells)
            {
                var piece = cell.CurrentPiece;
                if (piece == null || !piece.gameObject.activeSelf ||
                    !piece.TryGetComponent<IExplodable>(out var explodable))
                {
                    _explosionCount--;
                }
                else
                {
                    var distance = Mathf.Abs(piece.Row - row) + Mathf.Abs(piece.Col - col);
                    StartCoroutine(ExplodeDelayed(explodable, distance * delayPerDistance));
                }
            }
        }

        private IEnumerator ExplodeDelayed(IExplodable explodable, float delay)
        {
            yield return new WaitForSeconds(delay);
            explodable.TryExplode();
            _explosionCount--;
            if (_explosionCount == 0)
            {
                _dirtyTracker.ClearAll();
                CompleteCombination();
            }
        }
    }
}