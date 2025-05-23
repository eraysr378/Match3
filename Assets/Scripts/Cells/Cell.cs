using System;
using Managers;
using Misc;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cells
{
    public abstract class Cell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler
    {
        public static event Action<Cell> OnAnyPointerDown;
        public static event Action<Cell> OnAnyPointerUp;
        public static event Action<Cell> OnAnyPointerEnter;
        public static event Action<Cell> OnAnyRequestFill;
        public static event Action<Cell> OnAnyRequestFall;
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Piece CurrentPiece { get; private set; }
        protected CellType cellType;

        [SerializeField] private SpriteRenderer spriteRenderer;

        private int _dirtyCount = 0;

        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsDisabled())
            {
                return;
            }
            OnAnyPointerDown?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (IsDisabled())
            {
                return;
            }
            OnAnyPointerUp?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsDisabled())
            {
                return;
            }
            OnAnyPointerEnter?.Invoke(this);
        }
        public void SetPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public void SetPiece(Piece piece)
        {
            if (IsDisabled())
            {
                return;
            }
            CurrentPiece = piece;
        }
        private void RequestFillOrFall()
        {
            if (IsDisabled())
            {
                return;
            }
            if(Row == GridManager.Instance.Height-1 && CurrentPiece == null)
                OnAnyRequestFall?.Invoke(this);
            else
                OnAnyRequestFill?.Invoke(this);
        }

        public void FillIfClean()
        {
            if (_dirtyCount == 0)
            {
                RequestFillOrFall();
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
                RequestFillOrFall();
            }
        }

        public void SetColor(Color color)
        {
            spriteRenderer.color = color; 

        }

        public void ResetColor()
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.2f); 
        }

        public bool IsDisabled()
        {
            return cellType == CellType.Disabled;
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