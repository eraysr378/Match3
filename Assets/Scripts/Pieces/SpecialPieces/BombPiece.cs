using System;
using System.Collections;
using System.Collections.Generic;
using Cells;
using Interfaces;
using Managers;
using Misc;
using Pieces.Behaviors;
using UnityEngine;
using Utils;

namespace Pieces.SpecialPieces
{
    public class BombPiece : Piece, IActivatable, IExplodable, ICombinable
    {
        public event Action<IActivatable> OnActivationCompleted;
        [SerializeField] private int explosionRadius = 1;
        [SerializeField] private float explosionDelay = 0.1f;
        [SerializeField] private float destroyDelay = 0.5f;
        private readonly CellDirtyTracker _cellDirtyTracker = new();
        private SwapHandler _swapHandler;
        private FillHandler _fillHandler;

        protected  void Awake()
        {
            _swapHandler = GetComponent<SwapHandler>();
            _fillHandler = GetComponent<FillHandler>();
            _swapHandler.OnSwapStarted += SwapHandler_OnSwapStarted;
            _swapHandler.OnSwapCompleted += SwapHandler_OnSwapCompleted;
            _fillHandler.OnFillStarted += FillHandler_OnFillStarted;
            _fillHandler.OnFillCompleted += FillHandler_OnFillCompleted;
        }

        private void FillHandler_OnFillCompleted()
        {
            ClearOperation();
        }

        private void FillHandler_OnFillStarted()
        {
            SetOperation(PieceOperation.Filling);
        }

        private void SwapHandler_OnSwapCompleted(Piece obj)
        {
            TryActivate();
        }

        private void SwapHandler_OnSwapStarted()
        {
            SetOperation(PieceOperation.Swapping);
        }

        public bool TryActivate()
        {
            return TriggerExplosion(0);
        }

        public bool TryExplode()
        {
            return TriggerExplosion(explosionDelay);
        }

        private bool TriggerExplosion(float delay)
        {
            if (isBeingDestroyed)
                return false;
            SetOperation(PieceOperation.Activating);
            isBeingDestroyed = true;
            StartCoroutine(TriggerExplosionCoroutine(delay));
            return true;
        }

        private IEnumerator TriggerExplosionCoroutine(float delay)
        {
            EventManager.OnPieceActivated?.Invoke(this);

            yield return new WaitForSeconds(delay);
            PlayParticle();
            var cellsInRadius = GridManager.Instance.GetCellsInRadius(Row, Col, explosionRadius);
            List<IExplodable> explodables =
                GridManager.Instance.GetPiecesInRadius<IExplodable>(Row, Col, explosionRadius);
            explodables.Remove(this);
            _cellDirtyTracker.Mark(cellsInRadius);
            foreach (var explodable in explodables)
            {
                explodable.TryExplode();
            }

            Invoke(nameof(OnExplosionCompleted), destroyDelay);
        }

        private void PlayParticle()
        {
            var particle = EventManager.OnParticleSpawnRequested?.Invoke(ParticleType.BombExplosion, transform,
                transform.position, Vector3.one);
            particle?.transform.SetParent(null);
        }

        private void OnExplosionCompleted()
        {
            SetCell(null);
            _cellDirtyTracker.ClearAll();
            OnActivationCompleted?.Invoke(this);
            OnReturnToPool();
        }


        public override void OnSpawn()
        {
            base.OnSpawn();
        }

        public void OnCombined(Piece piece)
        {
        }
    }
}