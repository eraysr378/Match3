using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Pieces
{
    public class RainbowPiece : InteractablePiece, IActivatable, ISwappable, IExplodable, ICombinable
    {
        public event Action<IActivatable> OnActivationCompleted;
        public bool IsActivated { get; private set; }
        private PieceAnimationHandler _animationHandler;
        private PieceType _targetType;

        protected override void Awake()
        {
            base.Awake();
            _targetType = PieceTypeHelper.GetRandomNormalPieceType();
            _animationHandler = GetComponent<PieceAnimationHandler>();
        }

        public void Activate()
        {
            if (IsActivated) return;
            IsActivated = true;
            EventManager.OnPieceActivated?.Invoke(this);
            _animationHandler.PlayActivateAnimation(null);
            StartCoroutine(DestroyMatchingPieces(_targetType));
        }

        public void Explode()
        {
            Activate();
        }

        public void OnSwap(Piece otherPiece)
        {
            _targetType = otherPiece.GetPieceType();
            Activate();
        }

        private IEnumerator DestroyMatchingPieces(PieceType targetType)
        {
            Grid grid = GridManager.Instance.Grid;
            List<Piece> matchingPieces = grid.GetPiecesByType(targetType);
            matchingPieces = matchingPieces.OrderBy(_ => UnityEngine.Random.value).ToList();

            foreach (var piece in matchingPieces)
            {
                if (piece != null && piece.gameObject.activeSelf &&
                    piece.TryGetComponent<IExplodable>(out var explodable))
                    explodable.Explode();

                yield return new WaitForSeconds(0.1f);
            }

            _animationHandler.PlayEndAnimation(CompleteDestruction);
        }

        private void CompleteDestruction()
        {
            SetCell(null);
            OnActivationCompleted?.Invoke(this);
            ReturnToPool();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            IsActivated = false;
            _targetType = PieceTypeHelper.GetRandomNormalPieceType();
        }

        public void OnCombined(Piece piece)
        {
        }
    }
}