using System;
using System.Collections;
using Cells;
using Interfaces;
using Pieces.Behaviors;
using ScriptableObjects;
using UnityEngine;

namespace Pieces
{
    public class NormalPiece : Piece, ISwappable, IMatchable,IExplodable
    {
        public event Action OnMatchHandled;
        [SerializeField] private float specialMatchMergeDuration;
        [SerializeField] private float destructionDelay;
        [SerializeField] private NormalPieceSpritesSo spritesSo;
        private Movable _movable;

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

        private void SetPieceAppearance()
        {
            (visual.sprite,visual.color) = spritesSo.GetAppearance(pieceType);
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
            HandleDestruction(Color.red,destructionDelay);
        }
        public void OnNormalMatch()
        {
            HandleDestruction(Color.red,destructionDelay);
        }

        public void OnSpecialMatch(Cell spawnCell)
        {
            _collider2D.enabled = false;
            _movable.StartMoving(spawnCell.transform.position, specialMatchMergeDuration);

            _movable.OnTargetReached += OnMoveComplete;
            return;

            void OnMoveComplete()
            {
                _movable.OnTargetReached -= OnMoveComplete;
                StartCoroutine(DestroyAfter(destructionDelay));
            }
        }
        private void HandleDestruction(Color color, float delay)
        {
            _collider2D.enabled = false;
            visual.color = color;
            StartCoroutine(DestroyAfter(delay));
        }

       
    }
}