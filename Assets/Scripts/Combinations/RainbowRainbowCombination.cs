using System.Collections;
using System.Collections.Generic;
using CameraRelated;
using Cells;
using Interfaces;
using Managers;
using Misc;
using ParticleEffects;
using Pieces;
using UnityEngine;
using UnityEngine.Serialization;
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
            EventManager.OnBigCameraShakeRequest?.Invoke();
            DestroyAll(row, col);
        }

        private void DestroyAll(int row, int col)
        {
            List<BaseCell> allCells = GridManager.Instance.GetAllCells();
            _dirtyTracker.Mark(allCells);
            float delayPerDistance = 0.05f;
            _explosionCount = allCells.Count;
            foreach (var cell in allCells)
            {
                var distance = Mathf.Abs(cell.Row - row) + Mathf.Abs(cell.Col - col);
                StartCoroutine(ExplodeDelayed(cell, distance * delayPerDistance));
            }
        }

        private IEnumerator ExplodeDelayed(BaseCell cell, float delay)
        {
            yield return new WaitForSeconds(delay);
            cell.TriggerExplosion();
            _explosionCount--;
            if (_explosionCount == 0)
            {
                _dirtyTracker.ClearAll();
                CompleteCombination();
            }
        }
    }
}