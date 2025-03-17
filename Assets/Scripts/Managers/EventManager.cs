using System;
using System.Collections.Generic;
using Pieces;
using Misc;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public static class EventManager
    {
        public static Action<Piece> OnPointerDownCell;
        public static Action<Piece> OnPointerUpCell;
        public static Action<Piece> OnPointerEnterCell;
        public static Action<Piece> OnPointerClickedCell;
        public static Action<Grid> OnGridInitialized;
        public static Func<PieceType,int,int,Piece> OnRequestCellSpawn;
        public static Func<int,int,Piece> OnRequestRandomNormalCellSpawn;
        public static Action OnFillCompleted;
        public static Action OnValidMatchCleared;
        public static Action<Piece,Piece> OnSwapCompleted;
        public static Action<Piece> OnCellReturnToPool;


    }
}
