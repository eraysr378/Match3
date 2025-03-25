using System;
using Cells;
using Interfaces;
using Pieces;
using Misc;
namespace Managers
{
    public static class EventManager
    {
        public static Action<Piece,Piece> OnSwapRequested;
        public static Action<Piece,Piece> OnInstantSwapRequested;
        
        public static Func<PieceType,int,int,Piece> OnPieceSpawnRequested;
        public static Func<CellType,int,int,Cell> OnCellSpawnRequested;
        public static Func<int,int,Piece> OnRandomNormalPieceSpawnRequested;
        
        public static Action<Piece> OnPieceReturnToPool;
        
        public static Action<IActivatable> OnPieceActivated;
    }
}
