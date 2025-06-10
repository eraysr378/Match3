using Managers;
using Misc;
using Pieces;
using Pieces.SpecialPieces;

namespace Combinations
{
    public class RainbowRocketCombination : BaseRainbowSpecialPieceCombination<RocketPiece>
    {
        protected override RocketPiece SpawnSpecialPiece(int row, int col)
        {
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, row, col);
            RocketPiece rocketPiece = piece as RocketPiece;
            return rocketPiece;
        }

        protected override void ActivateAllSpecialPieces()
        {
            base.ActivateAllSpecialPieces();
            CompleteCombination();
        }
    }
}