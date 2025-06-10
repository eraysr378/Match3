using System;
using System.Collections;
using Cells;
using Interfaces;
using Managers;
using Misc;
using Pieces.Behaviors;
using Processes;
using UnityEngine;
using UnityEngine.Serialization;
using VisualEffects;

namespace Pieces.SpecialPieces
{
    public class RainbowPiece : Piece, IActivatable, IExplodable, ICombinable
    {
        public event Action<IActivatable> OnActivationCompleted;
        
        [SerializeField] private RainbowPieceLineRenderer rainbowPieceLineRendererPrefab;
        [SerializeField] private Transform radiusReferenceTransform;
        [SerializeField] private float destroyDuration;
        [SerializeField] private float timeBetweenDestroys;
        private PieceAnimationHandler _animationHandler;
        private PieceType _targetType;
        private float _radius;
        private SwapHandler _swapHandler;
        private FillHandler _fillHandler;

        protected  void Awake()
        {
            _targetType = PieceTypeHelper.GetRandomNormalPieceType();

            _animationHandler = GetComponent<PieceAnimationHandler>();
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

        private void SwapHandler_OnSwapCompleted(Piece otherPiece)
        {
            _targetType = otherPiece.GetPieceType();
            TryActivate();
            SetOperation(PieceOperation.Activating);

        }

        private void SwapHandler_OnSwapStarted()
        {
            SetOperation(PieceOperation.Swapping);
        }

        private void Start()
        {
            _radius = Vector2.Distance(radiusReferenceTransform.position, transform.position);
        }

        public bool TryActivate()
        {
            if (isBeingDestroyed) return false;
            isBeingDestroyed = true;
            SetOperation(PieceOperation.Activating);

            CurrentCell.MarkDirty();
            EventManager.OnPieceActivated?.Invoke(this);
            _animationHandler.PlayActivateAnimation(null);
            StartCoroutine(DestroyTargetPieces(_targetType));
            return true;
        }

        public bool TryExplode()
        {
            if (isBeingDestroyed)
                return false;

            TryActivate();
            return true;
        }


        private IEnumerator DestroyTargetPieces(PieceType targetType)
        {
            float elapsedTime = 0f;
            while (elapsedTime < destroyDuration)
            {
                Piece piece = GridManager.Instance.GetPieceOfType(targetType);
                // No more pieces of targetType left we are done
                if (piece == null)
                    break;

                StartDestroyProcessForPiece(piece);
                elapsedTime += timeBetweenDestroys;
                yield return new WaitForSeconds(timeBetweenDestroys);
            }

            _animationHandler.PlayEndAnimation(CompleteDestruction);
        }

        private void StartDestroyProcessForPiece(Piece piece)
        {
            var visualEffect = EventManager.OnVisualEffectSpawnRequested(VisualEffectType.RainbowPieceLineRenderer);
            if (visualEffect is not IRainbowLineEffect lineEffect)
            {
                Debug.LogError("Effect is not rainbow line effect");
                return;
            }

            lineEffect.SetUpAtEdge(transform, piece.CurrentCell.transform, _radius, transform.lossyScale.x);
            if (piece is IColorProvider colorProvider)
            {
                lineEffect.SetColor(colorProvider.GetColor());
            }

            var process = new RainbowDestroyProcess(piece, visualEffect);
            process.Execute();
        }

        private void CompleteDestruction()
        {
            BaseCell cellToClear = CurrentCell;
            SetCell(null);
            cellToClear.TriggerExplosion();
            cellToClear.ClearDirty();
            OnActivationCompleted?.Invoke(this);
            OnReturnToPool();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            _targetType = PieceTypeHelper.GetRandomNormalPieceType();
        }


        public void OnCombined(Piece piece)
        {
        }
    }
}