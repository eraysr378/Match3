using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Managers;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Pieces
{
    public class BombPiece : InteractablePiece, IActivatable, ISwappable, IExplodable,ICombinable
    {
        public event Action<IActivatable> OnActivationCompleted;
        public bool IsActivated { get; private set; }
        [SerializeField] private int explosionRadius = 1;
        [SerializeField] private float explosionDelay = 0.1f;
        [SerializeField] private float destroyDelay = 0.5f;
        private bool _isExploded;

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
            
            yield return new WaitForSeconds(delay);

            List<IExplodable> explodables = GridManager.Instance.GetPiecesInRadius<IExplodable>( Row, Col, explosionRadius);
            explodables.Remove(this);
            foreach (var explodable in explodables)
            {
                explodable.Explode();
            }

            Invoke(nameof(OnExplosionCompleted), destroyDelay);
            yield break;
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
        

        public void OnSwap(Piece otherPiece)
        {
            Activate();
        }

        public void OnCombined(Piece piece)
        {
            
        }
    }
}