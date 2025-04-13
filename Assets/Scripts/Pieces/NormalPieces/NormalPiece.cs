using System;
using System.Collections;
using Cells;
using Controllers;
using Interfaces;
using Managers;
using ParticleEffects;
using Pieces.Behaviors;
using UnityEngine;

namespace Pieces.NormalPieces
{
    [RequireComponent(typeof(Fillable))]
    public abstract class NormalPiece : InteractablePiece, ISwappable, IMatchable, IExplodable, IRainbowHittable,
        IColorProvider
    {
        public event Action OnRainbowHitHandled;
        public event Action OnMatchHandled;
        [SerializeField] private VisualController visualController;
        private readonly float _specialMatchMergeDuration = 0.25f;
        private Movable _movable;
        private Fillable _fillable;


        protected override void Awake()
        {
            base.Awake();
            _movable = GetComponent<Movable>();
            _fillable = GetComponent<Fillable>();
        }

        protected virtual void PlayParticleEffect()
        {
            Debug.LogWarning("No particle");
        }
        public override void OnSpawn()
        {
            base.OnSpawn();
            _fillable.enabled = true;
            visualController.ResetScale();
            visualController.EnableVisual();

        }

        public bool TryExplode()
        {
            if (isBeingDestroyed) return false;
            isBeingDestroyed = true;
            SetCell(null);
            visualController.DisableVisual();
            PlayParticleEffect();
            OnReturnToPool();
            return true;
        }
     
        public bool TryHandleRainbowHit(Action onHandled)
        {
            if (isBeingDestroyed) return false;
            isBeingDestroyed = true;
            SetCell(null);
            OnRainbowHitHandled += onHandled;
            visualController.Shrink(OnHitByRainbowHandled);
            return true;
        }

        private void OnHitByRainbowHandled()
        {
            CompleteDestruction(ref OnRainbowHitHandled);
        }

        public bool TryHandleNormalMatch(Action onHandled)
        {
            if (isBeingDestroyed) return false;
            isBeingDestroyed = true;
            SetCell(null);
            OnMatchHandled += onHandled;
            visualController.Shrink(OnNormalMatchHandled);
            return true;
        }

        private void OnNormalMatchHandled()
        {
            CompleteDestruction(ref OnMatchHandled);
        }

        private void CompleteDestruction(ref Action destructionEvent)
        {
            visualController.DisableVisual();

            destructionEvent?.Invoke();
            destructionEvent = null; // cleanup
            PlayParticleEffect();
            OnReturnToPool();
        }

        public bool TryHandleSpecialMatch(Cell spawnCell, Action onHandled)
        {
            if (isBeingDestroyed) return false;
            isBeingDestroyed = true;
            _fillable.enabled = false;
            SetCell(null);
            OnMatchHandled += onHandled;

            _movable.StartMovingWithDuration(spawnCell.transform.position, _specialMatchMergeDuration,
                onComplete: HandleMoveComplete);

            return true;
        }

        private void HandleMoveComplete()
        {
            OnMatchHandled?.Invoke();
            OnReturnToPool();
        }
        
        public void OnSwap(Piece otherPiece)
        {
            EventManager.OnMatchCheckRequested?.Invoke(this);
        }

        public void OnPostSwap(Piece otherPiece)
        {
            // dont need anything
        }

        public Color GetColor()
        {
            return visualController.GetColor();
        }
    }
}