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
    [RequireComponent(typeof(Movable))]
    public class RocketProjectile : MonoBehaviour
    {
        public event Action OnPathCleared;
        public event Action OnOutOfView;
        [SerializeField]private Vector3 defaultPosition;
        private float _moveSpeed;
        private Queue<Cell> _cellPath;
        private Movable _movable;
        private Cell _currentTargetCell;

        private void Awake()
        {
            _movable = GetComponent<Movable>();
        }
        public void Launch(List<Cell> path, float moveSpeed)
        {
            _moveSpeed = moveSpeed;
            if (path == null)
            {
                WhenPathCleared();
                return;
            }
            _cellPath = new Queue<Cell>(path);
            StartClearingPath();
        }

        private void StartClearingPath()
        {
            if (_cellPath.Count == 0)
            {
                WhenPathCleared();
                return;
            }
            _currentTargetCell = _cellPath.Dequeue();
            _movable.StartMovingWithSpeed(_currentTargetCell.transform.position,_moveSpeed,ExplodeCurrentCellPiece);
        }

        private void ExplodeCurrentCellPiece()
        {
            if (_currentTargetCell.CurrentPiece is IExplodable explodable)
            {
                explodable.TryExplode();
            }
            MoveToNextCell();
        }
        private void MoveToNextCell()
        {
            if (_cellPath.Count == 0)
            {
                WhenPathCleared();
                return;
            }
            _currentTargetCell = _cellPath.Dequeue();
            _movable.StartMovingWithSpeed(_currentTargetCell.transform.position,_moveSpeed,ExplodeCurrentCellPiece);
            
        }
        private IEnumerator ContinueStraight()
        {
            while (!IsOutOfView())
            {
                transform.position += (transform.up * (_moveSpeed * Time.deltaTime));
                yield return null;
            }
            OnOutOfView?.Invoke();
            gameObject.SetActive(false);
        }

        private void WhenPathCleared()
        {
            StartCoroutine(ContinueStraight());
            OnPathCleared?.Invoke();
        }
        
        public void Reset()
        {
            gameObject.SetActive(true);
            transform.localPosition = defaultPosition;
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