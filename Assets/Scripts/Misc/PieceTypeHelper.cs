using System;
using System.Collections.Generic;
using System.Linq;

namespace Misc
{
    public class PieceTypeHelper
    {
        private static readonly HashSet<PieceType> NormalCells;

        static PieceTypeHelper()
        {
            NormalCells = Enum.GetValues(typeof(NormalPieceType))
                .Cast<NormalPieceType>()
                .Select(type => (PieceType)type)
                .ToHashSet();
        }
        public static bool  IsNormalCell(PieceType pieceType) => NormalCells.Contains(pieceType);
    }
}