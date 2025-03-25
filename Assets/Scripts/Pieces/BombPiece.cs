using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Managers;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Pieces
{
    public class BombPiece : Piece, IActivatable, ISwappable, IExplodable
    {
        public event Action<IActivatable> OnActivationCompleted;
        public bool IsActivated { get; private set; }
        [SerializeField] private int explosionRadius = 1;
        [SerializeField] private float explosionDelay = 0.1f;
        [SerializeField] private float destroyDelay = 0.5f;

        public void Activate()
        {
            TriggerExplosion(0);
        }

        public void Explode()
        {
            TriggerExplosion(explosionDelay); // Add slight delay for explosion effect
        }

        private void TriggerExplosion(float delay)
        {
            if (IsActivated)
                return;

            StartCoroutine(TriggerExplosionCoroutine(delay));
        }

        private IEnumerator TriggerExplosionCoroutine(float delay)
        {
            EventManager.OnPieceActivated?.Invoke(this);
            IsActivated = true;
            visual.color = Color.red;

            yield return new WaitForSeconds(delay);

            Grid grid = GridManager.Instance.Grid;
            List<IExplodable> explodables = GetPiecesInRadius(grid, Row, Col, explosionRadius);

            foreach (var explodable in explodables)
            {
                explodable.Explode();
            }

            Invoke(nameof(OnExplosionCompleted), destroyDelay);
        }

        private void OnExplosionCompleted()
        {
            SetCell(null);
            OnActivationCompleted?.Invoke(this);
            ReturnToPool();
        }


        public override void OnSpawn()
        {
            base.OnSpawn();
            IsActivated = false;
        }


        private List<IExplodable> GetPiecesInRadius(Grid grid, int row, int col, int radius)
        {
            List<IExplodable> explodableList = new();

            for (int r = row - radius; r <= row + radius; r++)
            {
                for (int c = col - radius; c <= col + radius; c++)
                {
                    Piece piece = grid.GetCell(r, c)?.CurrentPiece;
                    if (piece != null && piece != this && piece.TryGetComponent<IExplodable>(out var explodable))
                    {
                        explodableList.Add(explodable);
                    }
                }
            }

            return explodableList;
        }
    }
}