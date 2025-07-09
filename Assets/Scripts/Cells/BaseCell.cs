using System;
using System.Collections.Generic;
using CellOverlays;
using Interfaces;
using Managers;
using Misc;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cells
{
    public abstract class BaseCell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler
    {
        public static event Action<BaseCell> OnAnyPointerDown;
        public static event Action<BaseCell> OnAnyPointerUp;
        public static event Action<BaseCell> OnAnyPointerEnter;
        public static event Action<BaseCell> OnAnyRequestFill;
        public static event Action<BaseCell> OnAnyRequestFall;
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Piece CurrentPiece { get; private set; }
        protected CellType cellType;


        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private int _dirtyCount = 0;
        private BaseCellOverlay _overlay;


        public void OnPointerDown(PointerEventData eventData)
        {
            OnAnyPointerDown?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnAnyPointerUp?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnAnyPointerEnter?.Invoke(this);
        }

        public void SetPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public void SetPiece(Piece piece)
        {
            CurrentPiece = piece;
        }

        public void TriggerExplosion()
        {
            if (CurrentPiece is IExplodable explodablePiece)
            {
                explodablePiece.TryExplode();
            }


            if (_overlay is IExplodable explodableOverlay)
            {
                explodableOverlay.TryExplode();
            }
        }

        public void TriggerAdjacentMatch()
        {
            if (CurrentPiece is IAdjacentMatchable adjacentMatchablePiece)
            {
                adjacentMatchablePiece.OnAdjacentMatch();
            }
            
            if (_overlay is IAdjacentMatchable adjacentMatchableOverlay)
            {
                adjacentMatchableOverlay.OnAdjacentMatch();
            }
        }

        public void TriggerRainbowHit()
        {
            // Rainbowhit is directly handled by piece itself if it cannot be handled by piece or there is no piece
            // this function will not be called so that it is safe
            
            if (_overlay is IRainbowHittable rainbowHittable)
            {
                rainbowHittable.TryHandleRainbowHit(null);
            }
        }

        public void TriggerAdjacentExplosion()
        {
            if (CurrentPiece is IAdjacentExplodable adjacentExplodablePiece)
            {
                adjacentExplodablePiece.OnAdjacentExplosion();
            }
            
            if (_overlay is IAdjacentExplodable adjacentExplodableOverlay)
            {
                adjacentExplodableOverlay.OnAdjacentExplosion();
            }
        }

        public void OnPieceMatched()
        {
            if (_overlay is IMatchReactive matchReactiveOverlay)
            {
                matchReactiveOverlay.OnMatch();
            }
        }
        public void SetOverlay(BaseCellOverlay overlay)
        {
            _overlay = overlay;
            overlay.OnDestroyed += OnOverlayDestroyed;
        }

        private void OnOverlayDestroyed(BaseCellOverlay overlay)
        {
            overlay.OnDestroyed -= OnOverlayDestroyed;
            _overlay = null;
        }

        private void RequestFillOrFall()
        {
            if (Row == GridManager.Instance.Height - 1 && CurrentPiece == null)
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
    }
}