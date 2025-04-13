using System;
using System.Collections;
using Cells;
using Interfaces;
using Managers;
using Misc;
using Processes;
using UnityEngine;
using UnityEngine.Serialization;
using VisualEffects;

namespace Pieces.SpecialPieces
{
    public class RainbowPiece : InteractablePiece, IActivatable, ISwappable, IExplodable, ICombinable
    {
        public event Action<IActivatable> OnActivationCompleted;
        [FormerlySerializedAs("pieceLineRendererPrefab")] [SerializeField] private RainbowPieceLineRenderer rainbowPieceLineRendererPrefab;
        [SerializeField] private Transform radiusReferenceTransform;
        [SerializeField] private float destroyDuration;
        [SerializeField] private float timeBetweenDestroys;
        private PieceAnimationHandler _animationHandler;
        private PieceType _targetType;
        private float _radius;


        protected override void Awake()
        {
            base.Awake();
            _targetType = PieceTypeHelper.GetRandomNormalPieceType();

            _animationHandler = GetComponent<PieceAnimationHandler>();
        }

        private void Start()
        {
            _radius = Vector2.Distance(radiusReferenceTransform.position, transform.position);
        }

        public void Activate()
        {
            if (isBeingDestroyed) return;
            isBeingDestroyed = true;

            CurrentCell.MarkDirty();
            EventManager.OnPieceActivated?.Invoke(this);
            _animationHandler.PlayActivateAnimation(null);
            StartCoroutine(DestroyTargetPieces(_targetType));
        }

        public bool TryExplode()
        {
            if (isBeingDestroyed)
                return false;

            Activate();
            return true;
        }

        public void OnSwap(Piece otherPiece)
        {
        }

        public void OnPostSwap(Piece otherPiece)
        {
            _targetType = otherPiece.GetPieceType();
            Activate();
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
            Cell cellToClear = CurrentCell;
            SetCell(null);
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