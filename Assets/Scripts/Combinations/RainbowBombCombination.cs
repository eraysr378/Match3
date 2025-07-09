using CameraRelated;
using Managers;
using Misc;
using Pieces;
using Pieces.SpecialPieces;
using UnityEngine;
using Utils;

namespace Combinations
{
    public class RainbowBombCombination : BaseRainbowSpecialPieceCombination<BombPiece>
    {
        protected override BombPiece SpawnSpecialPiece(int row, int col)
        {
            Piece piece = EventManager.RequestPieceSpawn?.Invoke(PieceType.BombPiece, row, col);
            BombPiece bombPiece= piece as BombPiece;
            return bombPiece;
        }

        protected override void ActivateAllSpecialPieces()
        {
            base.ActivateAllSpecialPieces();
            EventManager.OnBigCameraShakeRequest?.Invoke();
            CompleteCombination();
        }
    }
}