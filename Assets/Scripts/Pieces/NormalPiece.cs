using System;
using System.Collections;
using Cells;
using Interfaces;
using Pieces.Behaviors;
using ScriptableObjects;
using UnityEngine;

namespace Pieces
{
    public class NormalPiece : InteractablePiece, ISwappable, IMatchable, IExplodable
    {
        public event Action OnMatchHandled;
        [SerializeField] private float specialMatchMergeDuration;
        [SerializeField] private float destructionDelay;
        [SerializeField] private NormalPieceSpritesSo spritesSo;
        private Movable _movable;
        private bool _isExploded;

        protected override void Awake()
        {
            base.Awake();
            _movable = GetComponent<Movable>();
        }

        public override void Init(Vector3 position)
        {
            base.Init(position);
            SetPieceAppearance();
        }

        public override void Init(Cell cell)
        {
            base.Init(cell);
            SetPieceAppearance();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            _isExploded = false;
        }

        private void SetPieceAppearance()
        {
            (visual.sprite, visual.color) = spritesSo.GetAppearance(pieceType);
        }

        protected override IEnumerator DestroyAfter(float seconds)
        {
            SetCell(null);
            yield return new WaitForSeconds(seconds);
            OnMatchHandled?.Invoke();
            ReturnToPool();
        }

        public void Explode()
        {
            if (_isExploded) return;
            _isExploded = true;
            HandleDestruction(Color.red, destructionDelay);
        }

        public void OnNormalMatch()
        {
            HandleDestruction(Color.red, destructionDelay);
        }

        public void OnSpecialMatch(Cell spawnCell)
        {
            _collider2D.enabled = false;
            int multiplier = Mathf.Abs(spawnCell.Row - Row) + Mathf.Abs(spawnCell.Col - Col);
            float duration = multiplier * specialMatchMergeDuration;
            _movable.StartMoving(spawnCell.transform.position, duration,
                onComplete: OnMoveComplete);
        }

        private void OnMoveComplete()
        {
            StartCoroutine(DestroyAfter(destructionDelay));
        }

        private void HandleDestruction(Color color, float delay)
        {
            _collider2D.enabled = false;
            visual.color = color;
            StartCoroutine(DestroyAfter(delay));
        }


        public void OnSwap(Piece otherPiece)
        {
            // Dont do anything for now
        }
    }
}