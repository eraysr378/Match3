using System;
using Cells;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;

namespace Pieces
{
    public abstract class Piece : MonoBehaviour, IPoolableObject
    {
        public event Action OnDestroy;
        public BaseCell CurrentCell { get; private set; }
        public int Row => CurrentCell.Row;
        public int Col => CurrentCell.Col;

        [SerializeField] private PieceType pieceType;

        private PieceOperation _activeOperation;

        protected bool isBeingDestroyed = false;
        
        

        public virtual void Init(Vector3 position)
        {
            transform.position = position;
            transform.localScale = Vector3.one;
        }

        public virtual void Init(BaseCell baseCell)
        {
            SetCell(baseCell);
            // transform.SetParent(cell?.transform);
            transform.position = baseCell.transform.position;
            transform.localScale = Vector3.one;
        }

        public void SetCell(BaseCell newBaseCell)
        {
            CurrentCell?.SetPiece(null);
            CurrentCell = newBaseCell;
            CurrentCell?.SetPiece(this);

            // if (CurrentCell != null)
            // {
            //     // transform.SetParent(CurrentCell.transform);
            //     transform.localScale = Vector3.one;
            //     // transform.position = CurrentCell.transform.position;
            //     CurrentCell.SetPiece(this);
            // }
        }

        public PieceType GetPieceType() => pieceType;

        public void DestroyPieceInstantly()
        {
            SetCell(null);
            OnReturnToPool();
        }

        public virtual void OnSpawn()
        {
            isBeingDestroyed = false;
            ClearOperation();
        }

        public virtual void OnReturnToPool()
        {
            OnDestroy?.Invoke();
            EventManager.ReturnPieceToPool?.Invoke(this);
        }

        protected void SetOperation(PieceOperation operation)
        {
            _activeOperation = operation;
        }

        public PieceOperation GetActiveOperation()
        {
            return _activeOperation;
        }

        public bool IsBusy()
        {
            return _activeOperation != PieceOperation.None;
        }

        protected void ClearOperation()
        {
            _activeOperation = PieceOperation.None;
        }
    }
}