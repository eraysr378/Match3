using System;
using Pieces;
using UnityEngine;

namespace Projectiles
{
    // TO DO: object pool
    public class RainbowProjectile : MonoBehaviour
    {
        public event Action<RainbowProjectile, Piece> OnReachedTarget;
        [SerializeField] private float speed;
        [SerializeField] private SpriteRenderer visual;

        private Piece _targetPiece;
        private bool _isMoving = false;

        public void Initialize(Piece targetPiece)
        {
            _targetPiece = targetPiece;
            _isMoving = true;
        }

        private void Update()
        {
            if (_isMoving)
            {
                MoveProjectileToTargetPiece();
            }
        }

        private void MoveProjectileToTargetPiece()
        {
            float step = speed * Time.deltaTime;
            Vector3 targetPosition = _targetPiece.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f || !_targetPiece.gameObject.activeSelf)
            {
                _isMoving = false;
                OnReachedTarget?.Invoke(this, _targetPiece);
                Destroy(gameObject);
            }
        }
        
    }
}