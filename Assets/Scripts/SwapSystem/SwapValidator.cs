using System;
using Interfaces;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;

namespace SwapSystem
{
    public class SwapValidator
    {
        public static event Func<Piece, Piece, bool> OnSwapWillCauseMatch;


        public bool IsValidSwap(Piece firstPiece, Piece secondPiece)
        {
            
            if (firstPiece == null || secondPiece == null)
                return false;
   
            if (firstPiece == null || secondPiece == null)
            {
                Debug.LogError("That should not happen");
                return false;
            }
            bool isAnyActivatable = firstPiece is IActivatable || secondPiece is IActivatable;
            bool isFormingMatch = OnSwapWillCauseMatch?.Invoke(firstPiece, secondPiece) ?? false;

            return isAnyActivatable || isFormingMatch;
        }
    }
}