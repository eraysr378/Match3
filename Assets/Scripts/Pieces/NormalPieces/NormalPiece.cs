using System;
using System.Collections;
using Cells;
using Interfaces;
using ParticleEffects;
using Pieces.Behaviors;
using ScriptableObjects;
using UnityEngine;

namespace Pieces
{
    public class NormalPiece : InteractablePiece, ISwappable, IMatchable, IExplodable
    {
        public event Action OnMatchHandled;

        [SerializeField] private SpriteRenderer visual;
        [SerializeField] private float specialMatchMergeDuration;
        [SerializeField] private float destructionDelay;
        [SerializeField] private NormalPieceSpritesSo spritesSo;
        [SerializeField] private ParticleHandler particleHandler;

        private Movable _movable;
        private bool _isExploded;
        private Color _baseColor;

        protected override void Awake()
        {
            base.Awake();
            _movable = GetComponent<Movable>();
            _baseColor = visual.color;
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
            visual.color = _baseColor;
        }

        private void SetPieceAppearance()
        {
            (visual.sprite, visual.color) = spritesSo.GetAppearance(pieceType);
        }

        protected override IEnumerator DestroyAfter(float seconds)
        {
            SetCell(null);
            yield return new WaitForSeconds(seconds);
            particleHandler.Play(ParticleType.Explosion);
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
            SetCell(null);
            OnMatchHandled?.Invoke();
            ReturnToPool();
        }

        private void HandleDestruction(Color color, float delay)
        {
            _collider2D.enabled = false;
            visual.color = color;
            SetCell(null);
            particleHandler.Play(ParticleType.Explosion);
            OnMatchHandled?.Invoke();
            ReturnToPool();
            // StartCoroutine(DestroyAfter(delay));
        }


        public void OnSwap(Piece otherPiece)
        {
            // Dont do anything for now
        }
    }
}