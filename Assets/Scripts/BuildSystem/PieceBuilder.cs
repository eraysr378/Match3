using Cells;
using GridRelated;
using Interfaces;
using Managers;
using Misc;
using Pieces;

namespace BuildSystem
{
    public class PieceBuilder : IGridBuildProcess
    {
        public void Build(GridDataSo gridData, Grid grid)
        {
            foreach (var pieceData in gridData.pieceDataArray)
            {
                PieceType pieceType = pieceData.pieceType;
                int row = pieceData.row;
                int col = pieceData.column;
                
                BaseCell baseCell = grid.GetCellAt(row, col);
                Piece createdPiece = EventManager.RequestPieceSpawn?.Invoke(pieceType, row, col);
               
                baseCell.SetPiece(createdPiece);
                createdPiece?.Init(baseCell);
            }
        }
    }
}