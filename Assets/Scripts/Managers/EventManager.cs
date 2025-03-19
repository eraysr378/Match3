using System;
using System.Collections.Generic;
using Cells;
using Pieces;
using Misc;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public static class EventManager
    {
        public static Action<Piece> OnPointerDownPiece;
        public static Action<Piece> OnPointerUpPiece;
        public static Action<Piece> OnPointerEnterPiece;
        public static Action<Piece> OnPointerClickedPiece;
        public static Action<Grid> OnGridInitialized;
        public static Func<PieceType,int,int,Piece> OnRequestPieceSpawn;
        public static Func<CellType,int,int,Cell> OnRequestCellSpawn;
        public static Func<int,int,Piece> OnRequestRandomNormalPieceSpawn;
        public static Action OnFillCompleted;
        public static Action OnValidMatchCleared;
        public static Action<Piece,Piece> OnSwapCompleted;
        public static Action<Piece> OnPieceReturnToPool;


    }
}
