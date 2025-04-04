using System;

namespace Misc
{
    public class PieceTypeHelper
    {
        public static PieceType GetRandomNormalPieceType()
        {
            Array values = Enum.GetValues(typeof(NormalPieceType));
            var random = new Random();
            NormalPieceType randomNormalPiece = (NormalPieceType)values.GetValue(random.Next(values.Length));
            PieceType randomPieceType = (PieceType)randomNormalPiece;
            return randomPieceType;
        }

    }
}