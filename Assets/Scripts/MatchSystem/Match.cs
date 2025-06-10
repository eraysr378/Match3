using System.Collections.Generic;
using Cells;
using Misc;
using Pieces;

namespace MatchSystem
{
    public class Match
    {
        public List<Piece> PieceList { get; }
        public MatchShape Shape{ get; }
        public BaseCell OriginCell{ get; }
        public int Length { get; }

        public Match(List<Piece> pieceList, MatchShape shape, BaseCell originCell)
        {
            PieceList = pieceList;
            Shape = shape;
            OriginCell = originCell;
            Length = PieceList.Count;
        }

    }
}