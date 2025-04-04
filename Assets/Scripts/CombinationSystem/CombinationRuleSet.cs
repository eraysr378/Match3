using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace Pieces
{
    public class CombinationRuleSet
    {
        private readonly Dictionary<(PieceType, PieceType), PieceType> _combinationRules = new();

        public CombinationRuleSet()
        {
            AddRule(PieceType.RocketPiece, PieceType.BombPiece, PieceType.RocketBombCombination);
            
            AddRule(PieceType.BombPiece, PieceType.BombPiece, PieceType.BombBombCombination);
            
            AddRule(PieceType.RocketPiece, PieceType.RainbowPiece, PieceType.RocketBombCombination);
            
            AddRule(PieceType.BombPiece, PieceType.RainbowPiece, PieceType.RocketBombCombination);
  
        }

        private void AddRule(PieceType typeA, PieceType typeB, PieceType result)
        {
            _combinationRules[(typeA, typeB)] = result;
            _combinationRules[(typeB, typeA)] = result;
        }

        public bool TryGetCombinationResult(PieceType typeA, PieceType typeB, out PieceType combination)
        {
            return _combinationRules.TryGetValue((typeA, typeB), out combination);
        }
    }
}