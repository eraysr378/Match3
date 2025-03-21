using System;
using UnityEngine;
using Pieces;
using Interfaces;

namespace Managers
{
    public class ActivationManager : MonoBehaviour
    {
        public event Action<Piece> OnPieceActivated;

        public bool TryActivatePiece(Piece piece)
        {
            if (piece is IActivatable activatable)
            {
                activatable.Activate();
                OnPieceActivated?.Invoke(piece);
                return true;
            }
            return false;
        }

        // public void ActivatePieces(params Piece[] pieces)
        // {
        //     foreach (var piece in pieces)
        //     {
        //         TryActivatePiece(piece);
        //     }
        // }
    }
}