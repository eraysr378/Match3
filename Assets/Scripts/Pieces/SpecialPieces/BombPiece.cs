using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;
using Utils;

namespace Pieces.SpecialPieces
{
    public class BombPiece : InteractablePiece, IActivatable, ISwappable, IExplodable,ICombinable
    {
        public event Action<IActivatable> OnActivationCompleted;
        [SerializeField] private int explosionRadius = 1;
        [SerializeField] private float explosionDelay = 0.1f;
        [SerializeField] private float destroyDelay = 0.5f;
        private readonly CellDirtyTracker _cellDirtyTracker = new();
        public void Activate()
        {
            TriggerExplosion(0);
        }

        public bool TryExplode()
        {
            return TriggerExplosion(explosionDelay);
        }

        private bool TriggerExplosion(float delay)
        {
            if (isBeingDestroyed)
                return false;
            isBeingDestroyed = true;
            StartCoroutine(TriggerExplosionCoroutine(delay));
            return true;
        }

        private IEnumerator TriggerExplosionCoroutine(float delay)
        {
            EventManager.OnPieceActivated?.Invoke(this);
            
            yield return new WaitForSeconds(delay);
            PlayParticle();
            var cellsInRadius = GridManager.Instance.GetCellsInRadius(Row, Col,explosionRadius);
            List<IExplodable> explodables = GridManager.Instance.GetPiecesInRadius<IExplodable>( Row, Col, explosionRadius);
            explodables.Remove(this);
            _cellDirtyTracker.Mark(cellsInRadius);
            foreach (var explodable in explodables)
            {
                explodable.TryExplode();
            }
            
            Invoke(nameof(OnExplosionCompleted), destroyDelay);
        }

        private void PlayParticle()
        {
            var particle = EventManager.OnParticleSpawnRequested?.Invoke(ParticleType.BombExplosion, transform,
                transform.position, Vector3.one);
            particle?.transform.SetParent(null);
        }

        private void OnExplosionCompleted()
        {
            SetCell(null);
            _cellDirtyTracker.ClearAll();
            OnActivationCompleted?.Invoke(this);
            OnReturnToPool();
        }


        public override void OnSpawn()
        {
            base.OnSpawn();
        }
        

        public void OnSwap(Piece otherPiece)
        {
            // dont need anything  
        }
        public void OnPostSwap(Piece otherPiece)
        {
            Activate();
        }
        public void OnCombined(Piece piece)
        {
            
        }
    }
}