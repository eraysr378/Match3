using System;
using System.Collections;
using CameraRelated;
using Interfaces;
using Managers;
using Misc;
using Pieces.Behaviors;
using SoundRelated;
using UnityEngine;
using Utils;

namespace Pieces.SpecialPieces
{
    public class BombPiece : Piece, IActivatable, IExplodable, ICombinable,IControlledActivatable
    {
        public event Action<IActivatable> OnActivationCompleted;
        [SerializeField] private int explosionRadius = 1;
        [SerializeField] private float explosionDelay = 0.1f;
        private readonly CellDirtyTracker _cellDirtyTracker = new();
        private SwapHandler _swapHandler;
        private FillHandler _fillHandler;
        private Shaker _shaker;

        protected  void Awake()
        {
            _swapHandler = GetComponent<SwapHandler>();
            _fillHandler = GetComponent<FillHandler>();
            _shaker = GetComponent<Shaker>();
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
            return TriggerExplosion();
        }

        public bool TryExplode()
        {
            return TriggerExplosion();
        }

        public void WaitForActivation()
        {
            SetOperation(PieceOperation.WaitingForActivation);
            _shaker.Shake();
        }
        public void ForceActivate()
        {
            ClearOperation();
            _shaker.Stop();
            TryActivate();
        }
        private bool IsWaitingForActivation()
        {
            return GetActiveOperation() == PieceOperation.WaitingForActivation;
        }
        private bool TriggerExplosion()
        {
            if (isBeingDestroyed || IsWaitingForActivation())
                return false;
            SetOperation(PieceOperation.Activating);
            isBeingDestroyed = true;
            SoundManager.Instance.PlaySound(SoundType.BombExplosion);
            StartCoroutine(TriggerExplosionCoroutine());
            return true;
        }

        private IEnumerator TriggerExplosionCoroutine()
        {
            EventManager.OnPieceActivated?.Invoke(this);
            
            PlayParticle();
            EventManager.OnSmallCameraShakeRequest?.Invoke();
            yield return new WaitForSeconds(explosionDelay);
            var cellsInRadius = GridManager.Instance.GetCellsInRadius(Row, Col, explosionRadius);
            _cellDirtyTracker.Mark(cellsInRadius);
            
            cellsInRadius.Remove(CurrentCell);
            foreach (var cell in cellsInRadius)
            {
                cell.TriggerExplosion();
            }
            OnExplosionCompleted();
        }

        private void PlayParticle()
        {
            var particle = EventManager.RequestParticleSpawn?.Invoke(ParticleType.BombExplosion,
                transform.position);
            particle?.Play();
        }

        private void OnExplosionCompleted()
        {
            var cell = CurrentCell;
            SetCell(null);
            cell.TriggerExplosion();
            _cellDirtyTracker.ClearAll();
            OnActivationCompleted?.Invoke(this);
            OnReturnToPool();
        }
        

        public void OnCombined(Piece piece)
        {
        }
    }
}