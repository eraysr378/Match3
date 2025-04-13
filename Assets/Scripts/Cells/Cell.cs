using System;
using Managers;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;

namespace Cells
{
    public abstract class Cell : MonoBehaviour
    {
        public static event Action<Cell> OnRequestFill;
        public static event Action<Cell> OnRequestFall;
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Piece CurrentPiece { get; private set; }

        [SerializeField] private SpriteRenderer spriteRenderer;

        private int _dirtyCount = 0;

        public void SetPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public void SetPiece(Piece piece)
        {
            CurrentPiece = piece;
        }
        private void RequestFill()
        {
            if(Row == GridManager.Instance.Height-1 && CurrentPiece == null)
                OnRequestFall?.Invoke(this);
            else
                OnRequestFill?.Invoke(this);
        }

        public void FillIfClean()
        {
            if (_dirtyCount == 0)
            {
                RequestFill();
            }
        }
        public bool IsDirty()
        {
            return _dirtyCount != 0;
        }
        public void MarkDirty()
        {
            _dirtyCount++;
        }
        public void ClearDirty()
        {
            _dirtyCount--;
            if (_dirtyCount == 0)
            {
                RequestFill();
            }
        }

        private void Update()
        {
            // if (CurrentPiece == null)
            // {
            //     spriteRenderer.color = new Color(1, 1, 1, 0.8f); 
            // }
            // else
            // {
            //     spriteRenderer.color = new Color(1, 1, 1, 0.2f); 
            // }
        }
    }

}