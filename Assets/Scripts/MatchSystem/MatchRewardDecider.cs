using Misc;

namespace MatchSystem
{
    public static class MatchRewardDecider
    {
        public static  PieceType DecideReward(Match match)
        {
            int count = match.PieceList.Count;
            MatchShape shape = match.Shape;

            if (shape == MatchShape.T || shape == MatchShape.L)
                return count >= 7 ? PieceType.RainbowPiece : PieceType.BombPiece;
        
            if (shape == MatchShape.Line && count >= 5)
                return PieceType.RainbowPiece;

            return PieceType.RocketPiece;
        }
    }
}