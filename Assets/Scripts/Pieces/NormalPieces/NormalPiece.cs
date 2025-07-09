using System;
using Cells;
using Interfaces;
using Managers;
using Misc;
using Pieces.Behaviors;
using UnityEngine;

namespace Pieces.NormalPieces
{
    [RequireComponent(typeof(FillHandler))]
    public abstract class NormalPiece : Piece, IMatchable, IExplodable, IRainbowHittable,
        IColorProvider
    {
        public event Action OnRainbowHitHandled;
        public event Action OnMatchHandled;

        [SerializeField] private SpriteRenderer visual;
        [SerializeField] private  SpriteRenderer providedColorSpriteRenderer;
        private Resizer _visualResizer;
        private Bouncer _visualBouncer;
        private readonly float _specialMatchMergeDuration = 0.25f;
        private Movable _movable;
        private FillHandler _fillHandler;
        private SwapHandler _swapHandler;
        private FallHandler _fallHandler;
        private GoalHandler _goalHandler;
        private ScoreHandler _scoreHandler;

        protected  void Awake()
        {
            _movable = GetComponent<Movable>();
            _fillHandler = GetComponent<FillHandler>();
            _swapHandler = GetComponent<SwapHandler>();
            _fallHandler = GetComponent<FallHandler>();
            _goalHandler = GetComponent<GoalHandler>();
            _scoreHandler = GetComponent<ScoreHandler>();
            _visualResizer = visual.GetComponent<Resizer>();
            _visualBouncer = visual.GetComponent<Bouncer>();
            
            _swapHandler.OnSwapStarted += SwapHandler_OnSwapStarted;
            _swapHandler.OnSwapCompleted += SwapHandler_OnSwapCompleted;
            
            _fillHandler.OnFillStarted += FillHandler_OnFillStarted;
            _fillHandler.OnFillCompleted += FillHandler_OnFillCompleted;
            
            _fallHandler.OnFallStarted += FallHandler_OnFallStarted;
            _fallHandler.OnFallCompleted += FallHandler_OnFallCompleted;
        }

        private void FallHandler_OnFallCompleted()
        {
            ClearOperation();
            _visualBouncer.Bounce();
            if (!isBeingDestroyed && !_fillHandler.TryStartFill() )
            {
               // EventManager.OnMatchCheckRequested?.Invoke(this); 
            }
        }

        private void FallHandler_OnFallStarted()
        {
            SetOperation(PieceOperation.Falling);
        }

        private void FillHandler_OnFillCompleted()
        {
            ClearOperation();
            _visualBouncer.Bounce();
            // if(!isBeingDestroyed)
            //     EventManager.OnMatchCheckRequested?.Invoke(this);
        }

        private void FillHandler_OnFillStarted()
        {
            _visualBouncer.CancelBounce();
            SetOperation(PieceOperation.Filling);

        }

        private void SwapHandler_OnSwapCompleted(Piece obj)
        {
            EventManager.RequestMatchCheck?.Invoke(this);
            ClearOperation();
            _fillHandler.TryStartFill();

        }

        private void SwapHandler_OnSwapStarted()
        {
            SetOperation(PieceOperation.Swapping);
        }

        protected virtual void PlayParticleEffect()
        {
            Debug.LogWarning("No particle");
        }
        public override void OnSpawn()
        {
            base.OnSpawn();
            _fillHandler.enabled = true;
            _visualResizer.ResetScale();
        }

        public bool TryExplode()
        {
            if (isBeingDestroyed) return false;
            isBeingDestroyed = true;
            SetCell(null);
            PlayParticleEffect();
            _scoreHandler.ReportScore(this);
            _goalHandler.ReportGoal();
            OnReturnToPool();
            return true;
        }
     
        public bool TryHandleRainbowHit(Action onHandled)
        {
            if (isBeingDestroyed) return false;
            isBeingDestroyed = true;
            SetCell(null);
            OnRainbowHitHandled += onHandled;
            _scoreHandler.ReportScore(this);
            _goalHandler.ReportGoal();
            _visualResizer.Resize(Vector3.zero,0.1f,OnHitByRainbowHandled);
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
            _scoreHandler.ReportScore(this);
            _goalHandler.ReportGoal();
            OnMatchHandled += onHandled;
            _visualResizer.ShrinkToZero(onComplete:OnNormalMatchHandled);
            return true;
        }

        private void OnNormalMatchHandled()
        {
            CompleteDestruction(ref OnMatchHandled);
        }

        private void CompleteDestruction(ref Action destructionEvent)
        {
            destructionEvent?.Invoke();
            destructionEvent = null; // cleanup
            PlayParticleEffect();
            OnReturnToPool();
        }

        public bool TryHandleSpecialMatch(BaseCell spawnBaseCell, Action onHandled)
        {
            if (isBeingDestroyed) return false;
            isBeingDestroyed = true;
            SetCell(null);
            _scoreHandler.ReportScore(this);
            _goalHandler.ReportGoal();
            OnMatchHandled += onHandled;
            _movable.StartMovingWithDuration(spawnBaseCell.transform.position, _specialMatchMergeDuration,
                onComplete: HandleMoveComplete);

            return true;
        }

        private void HandleMoveComplete()
        {
            OnMatchHandled?.Invoke();
            OnReturnToPool();
        }
        
    
        public Color GetColor()
        {
            return providedColorSpriteRenderer.color;
        }
    }
}