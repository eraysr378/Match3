using System;
using Interfaces;
using UnityEngine;

namespace Pieces
{
    public class RocketProjectile : MonoBehaviour
    {
        public event Action OnExitBorders;
        private Vector3 _defaultPosition;
        private Vector2 _direction;
        private float _speed;
        private Collider2D _collider;
        private bool _isLaunched;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void Reset()
        {
            _speed = 0;
            _isLaunched = false;
            gameObject.SetActive(true);
            transform.localPosition = _defaultPosition;
        }

        public void Launch(float speed, Collider2D rocketCollider)
        {
            _defaultPosition = transform.localPosition;
            _direction = transform.up;
            _speed = speed;
            Physics2D.IgnoreCollision(_collider, rocketCollider);
            _isLaunched = true;
        }

        private void Update()
        {
            if(!_isLaunched) return;
            
            Vector3 oldPosition = transform.position;
            transform.position += (Vector3)(_direction * (_speed * Time.deltaTime));

            RaycastHit2D[] hits = Physics2D.RaycastAll(oldPosition, _direction, _speed * Time.deltaTime);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != gameObject) // Ignore self
                {
                    HandleCollision(hit.collider);
                }
            }

            CheckIfOutOfView();
        }

        private void CheckIfOutOfView()
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            // Check if the object is outside the screen
            if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
            {
                OnExitBorders?.Invoke();
                gameObject.SetActive(false);
            }
        }

        private void HandleCollision(Collider2D hitCollider)
        {
            if (hitCollider.TryGetComponent(out IActivatable activatable))
            {
                activatable.Activate();
            }
            else if (hitCollider.TryGetComponent(out Piece piece))
            {
                piece.DestroyPiece();
            }
        }
    }
}