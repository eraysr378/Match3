using System.Collections.Generic;
using Misc;

namespace CombinationSystem
{
    public class CombinationRuleSet
    {
        private readonly Dictionary<(PieceType, PieceType), CombinationType> _combinationRules = new();

        public CombinationRuleSet()
        {
            AddRule(PieceType.RocketPiece, PieceType.BombPiece, CombinationType.RocketBombCombination);
            
            AddRule(PieceType.BombPiece, PieceType.BombPiece, CombinationType.BombBombCombination);
            
            AddRule(PieceType.RocketPiece, PieceType.RainbowPiece, CombinationType.RocketBombCombination);
            
            AddRule(PieceType.BombPiece, PieceType.RainbowPiece, CombinationType.RainbowBombCombination);
            
            AddRule(PieceType.RocketPiece, PieceType.RocketPiece, CombinationType.RocketRocketCombination);
            
            AddRule(PieceType.RocketPiece, PieceType.RainbowPiece, CombinationType.RainbowRocketCombination);
  
            AddRule(PieceType.RainbowPiece, PieceType.RainbowPiece, CombinationType.RainbowRainbowCombination);

        }

        private void AddRule(PieceType typeA, PieceType typeB, CombinationType result)
        {
            _combinationRules[(typeA, typeB)] = result;
            _combinationRules[(typeB, typeA)] = result;
        }

        public bool TryGetCombinationResult(PieceType typeA, PieceType typeB, out CombinationType combination)
        {
            return _combinationRules.TryGetValue((typeA, typeB), out combination);
        }
    }
}