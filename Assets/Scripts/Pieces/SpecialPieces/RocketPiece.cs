using System;
using System.Collections.Generic;
using Cells;
using Interfaces;
using Managers;
using Misc;
using Pieces.Behaviors;
using Projectiles;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Pieces.SpecialPieces
{
    public class RocketPiece : Piece, IActivatable, IExplodable, ICombinable,IControlledActivatable
    {
        public event Action<IActivatable> OnActivationCompleted;

        [SerializeField] private float projectileSpeed;
        [SerializeField] private RocketProjectile projectileLeft;
        [SerializeField] private RocketProjectile projectileRight;
        private bool _isVertical;
        private int _exitedProjectileCount;
        private int _outOfViewProjectileCount;
        private List<BaseCell> _leftPath;
        private List<BaseCell> _rightPath;
        private readonly CellDirtyTracker _cellDirtyTracker = new();
        private SwapHandler _swapHandler;
        private FillHandler _fillHandler;
        private Shaker _shaker;
        protected  void Awake()
        {
            _swapHandler = GetComponent<SwapHandler>();
            _fillHandler = GetComponent<FillHandler>();
            _shaker = GetComponent<Shaker>();
            _swapHandler.OnSwapStarted += SwapHandler_OnSwapStarted;
            _swapHandler.OnSwapCompleted += SwapHandler_OnSwapCompleted;
            _fillHandler.OnFillStarted += FillHandler_OnFillStarted;
            _fillHandler.OnFillCompleted += FillHandler_OnFillCompleted;
        }

        private void FillHandler_OnFillCompleted()
        {
            ClearOperation();
        }

        private void FillHandler_OnFillStarted()
        {
            SetOperation(PieceOperation.Filling);
        }

        private void SwapHandler_OnSwapCompleted(Piece obj)
        {
            TryActivate();
        }

        private void SwapHandler_OnSwapStarted()
        {
            SetOperation(PieceOperation.Swapping);
        }

        private void OnEnable()
        {
            projectileLeft.OnPathCleared += OnProjectilePathCleared;
            projectileRight.OnPathCleared += OnProjectilePathCleared;

            projectileLeft.OnReadyToDisable += OnProjectileReadyToDisable;
            projectileRight.OnReadyToDisable += OnProjectileReadyToDisable;
        }

        private void OnDisable()
        {
            projectileLeft.OnPathCleared -= OnProjectilePathCleared;
            projectileRight.OnPathCleared -= OnProjectilePathCleared;

            projectileLeft.OnReadyToDisable -= OnProjectileReadyToDisable;
            projectileRight.OnReadyToDisable -= OnProjectileReadyToDisable;
        }

        public override void Init(Vector3 position)
        {
            base.Init(position);
            SetRandomOrientation();
        }

        public override void Init(BaseCell baseCell)
        {
            base.Init(baseCell);
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

        private void OnProjectileReadyToDisable()
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
            return TryActivate();
        }

        public bool ActivateAt(BaseCell activatedBaseCell)
        {
            if (isBeingDestroyed || IsWaitingForActivation())
                return false;
            SetOperation(PieceOperation.Activating);
            isBeingDestroyed = true;
            if (activatedBaseCell == null)
            {
                _leftPath = null;
                _rightPath = null;
                EventManager.OnPieceActivated?.Invoke(this);
                LaunchProjectiles();
                return true;
            }
            _leftPath = new List<BaseCell>();
            _rightPath = new List<BaseCell>();
            _leftPath.Add(activatedBaseCell);
            if (_isVertical)
            {
                _leftPath.AddRange(GridManager.Instance.GetCellsAbove(activatedBaseCell.Row, activatedBaseCell.Col));
                _rightPath.AddRange(GridManager.Instance.GetCellsBelow(activatedBaseCell.Row, activatedBaseCell.Col)) ;
            }
            else
            {
                _leftPath.AddRange(GridManager.Instance.GetCellsLeft(activatedBaseCell.Row, activatedBaseCell.Col));
                _rightPath.AddRange(GridManager.Instance.GetCellsRight(activatedBaseCell.Row, activatedBaseCell.Col)) ;
            }
            _cellDirtyTracker.Mark(_leftPath);
            _cellDirtyTracker.Mark(_rightPath);
            EventManager.OnPieceActivated?.Invoke(this);
            LaunchProjectiles();
            return true;
        }

        public bool TryActivate()
        {
            return ActivateAt(CurrentCell);
        }
        public void WaitForActivation()
        {
            Debug.Log("Rocket is waiting for activation...");
            
            SetOperation(PieceOperation.WaitingForActivation);
            _shaker.Shake();
        }
        
        public void ForceActivate()
        {
            Debug.Log("Rocket is force activated");
            _shaker.Stop();
            ClearOperation();
            TryActivate();
        }
        private bool IsWaitingForActivation()
        {
            return GetActiveOperation() == PieceOperation.WaitingForActivation;
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
        public void OnCombined(Piece piece)
        {
        }
    }
}