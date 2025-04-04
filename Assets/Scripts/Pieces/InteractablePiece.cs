using System;
using Factories.PieceFactories;
using UnityEngine.EventSystems;

namespace Pieces
{
    public class InteractablePiece : Piece, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler
    {
        public static event Action<Piece> OnAnyPointerDown;
        public static event Action<Piece> OnAnyPointerUp;
        public static event Action<Piece> OnAnyPointerEnter;
        
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
    }
}