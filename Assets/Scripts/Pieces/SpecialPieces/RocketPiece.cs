using System;
using System.Collections.Generic;
using Cells;
using Interfaces;
using Managers;
using Projectiles;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Pieces.SpecialPieces
{
    public class RocketPiece : InteractablePiece, IActivatable, ISwappable, IExplodable, ICombinable
    {
        public event Action<IActivatable> OnActivationCompleted;

        [SerializeField] private float projectileSpeed;
        [SerializeField] private RocketProjectile projectileLeft;
        [SerializeField] private RocketProjectile projectileRight;
        private bool _isVertical;
        private int _exitedProjectileCount;
        private int _outOfViewProjectileCount;
        private List<Cell> _leftPath;
        private List<Cell> _rightPath;
        private readonly CellDirtyTracker _cellDirtyTracker = new();

        private void OnEnable()
        {
            projectileLeft.OnPathCleared += OnProjectilePathCleared;
            projectileRight.OnPathCleared += OnProjectilePathCleared;

            projectileLeft.OnOutOfView += OnProjectileOutOfView;
            projectileRight.OnOutOfView += OnProjectileOutOfView;
        }

        private void OnDisable()
        {
            projectileLeft.OnPathCleared -= OnProjectilePathCleared;
            projectileRight.OnPathCleared -= OnProjectilePathCleared;

            projectileLeft.OnOutOfView -= OnProjectileOutOfView;
            projectileRight.OnOutOfView -= OnProjectileOutOfView;
        }

        public override void Init(Vector3 position)
        {
            base.Init(position);
            SetRandomOrientation();
        }

        public override void Init(Cell cell)
        {
            base.Init(cell);
            SetRandomOrientation();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            projectileLeft.Reset();
            projectileRight.Reset();
            _exitedProjectileCount = 0;
            _outOfViewProjectileCount = 0;
        }

        private void OnProjectileOutOfView()
        {
            _outOfViewProjectileCount++;
            if (_outOfViewProjectileCount != 2) return;
            OnReturnToPool();
        }

        private void OnProjectilePathCleared()
        {
            _exitedProjectileCount++;
            if (_exitedProjectileCount != 2) return;
            SetCell(null);
            _cellDirtyTracker.ClearAll();
            OnActivationCompleted?.Invoke(this);
        }

        public bool TryExplode()
        {
            if (isBeingDestroyed)
                return false;
            Activate();
            return true;
        }

        public void ActivateAt(Cell activatedCell)
        {
            if (isBeingDestroyed)
                return;
            isBeingDestroyed = true;
            if (activatedCell == null)
            {
                _leftPath = null;
                _rightPath = null;
                EventManager.OnPieceActivated?.Invoke(this);
                LaunchProjectiles();
                return;
            }
            if (_isVertical)
            {
                _leftPath = GridManager.Instance.GetCellsAbove(activatedCell.Row, activatedCell.Col);
                _rightPath = GridManager.Instance.GetCellsBelow(activatedCell.Row, activatedCell.Col);
            }
            else
            {
                _leftPath = GridManager.Instance.GetCellsLeft(activatedCell.Row, activatedCell.Col);
                _rightPath = GridManager.Instance.GetCellsRight(activatedCell.Row, activatedCell.Col);
            }

            _cellDirtyTracker.Mark(activatedCell);
            _cellDirtyTracker.Mark(_leftPath);
            _cellDirtyTracker.Mark(_rightPath);

            EventManager.OnPieceActivated?.Invoke(this);
            LaunchProjectiles();
        }

        public void Activate()
        {
            ActivateAt(CurrentCell);
        }

        private void LaunchProjectiles()
        {
            transform.SetParent(null);
            projectileLeft.Launch(_leftPath, projectileSpeed);
            projectileRight.Launch(_rightPath, projectileSpeed);
        }


        private void SetRandomOrientation()
        {
            if (Random.Range(0, 2) == 1)
                SetVertical();
            else
                SetHorizontal();
        }

        public void SetHorizontal()
        {
            _isVertical = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public void SetVertical()
        {
            _isVertical = true;
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }

        public void OnSwap(Piece otherPiece)
        {
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