using System;
using System.Collections;
using System.Collections.Generic;
using Cells;
using Interfaces;
using Pieces.Behaviors;
using UnityEngine;
using UnityEngine.Serialization;

namespace Projectiles
{
    public enum Direction
    {
        Left,Right,Up,Down
    }
    [RequireComponent(typeof(Movable))]
    public class RocketProjectile : MonoBehaviour
    {
        public event Action OnPathCleared;
        public event Action OnReadyToDisable;
        [SerializeField] private Direction direction;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private Transform hitPoint;
        private float _moveSpeed;
        private Queue<BaseCell> _cellPath;
        private Movable _movable;
        private BaseCell _currentTargetBaseCell;
        private float _waitTimeBeforeDisabling = 1f;
        private Vector3 _hitOffset;

        private void Awake()
        {
            _movable = GetComponent<Movable>();
        }
        public void Launch(List<BaseCell> path, float moveSpeed)
        {
            _moveSpeed = moveSpeed;
            particle.gameObject.SetActive(true);
            if (path == null)
            {
                WhenPathCleared();
                return;
            }
            _cellPath = new Queue<BaseCell>(path);
            _hitOffset = hitPoint.position - transform.position;
            ClearPath();
        }

        private void ClearPath()
        {
            if (_cellPath.Count == 0)
            {
                WhenPathCleared();
                return;
            }
            _currentTargetBaseCell = _cellPath.Dequeue();
            Vector3 targetPosition = _currentTargetBaseCell.transform.position - _hitOffset;
            _movable.StartMovingWithSpeed(targetPosition,_moveSpeed,ExplodeCurrentCell);
        }

        private void ExplodeCurrentCell()
        {
            _currentTargetBaseCell.TriggerExplosion();
            ClearPath();
        }
 
        private IEnumerator ContinueStraight()
        {
            while (!IsOutOfView())
            {
                if (direction == Direction.Right)
                {
                    transform.position += (transform.right * (_moveSpeed * Time.deltaTime));
                }
                else
                {
                    transform.position += (-transform.right * (_moveSpeed * Time.deltaTime));
                }
                yield return null;
            }

            float elapsedTime = 0f;
            while (elapsedTime < _waitTimeBeforeDisabling)
            {
                elapsedTime += Time.deltaTime;
                if (direction == Direction.Right)
                {
                    transform.position += (transform.right * (_moveSpeed * Time.deltaTime));
                }
                else
                {
                    transform.position += (-transform.right * (_moveSpeed * Time.deltaTime));
                }
                yield return null;
            }
            Disable();
        }

        private void Disable()
        {
            OnReadyToDisable?.Invoke();
            gameObject.SetActive(false);
        }
        private void WhenPathCleared()
        {
            StartCoroutine(ContinueStraight());
            OnPathCleared?.Invoke();
        }
        
        public void Reset()
        {
            particle.gameObject.SetActive(false);
            gameObject.SetActive(true);
            transform.localPosition = Vector3.zero;
        }
        private bool IsOutOfView()
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            
            if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
            {
                return true;
            }
            return false;
        }
        
    }
}