using System;
using Interfaces;
using Pieces;

namespace SwapSystem
{
    public class SwapValidator
    {
        public static event Func<Piece, Piece, bool> OnSwapWillCauseMatch;


        public bool IsValidSwap(Piece first, Piece second)
        {

            if (first == null || second == null)
                return false;
            
            bool isAnyActivatable = first is IActivatable || second is IActivatable;
            bool isFormingMatch = OnSwapWillCauseMatch?.Invoke(first, second) ?? false;

            return isAnyActivatable || isFormingMatch;
        }
    }
}