using System;
using System.Collections.Generic;
using Cells;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public static class EventManager
    {
        public static Action<Cell> OnPointerDownCell;
        public static Action<Cell> OnPointerUpCell;
        public static Action<Cell> OnPointerEnterCell;
        public static Action<Cell> OnPointerClickedCell;
        public static Action<Grid> OnGridInitialized;
        public static Func<CellType,int,int,Cell> OnRequestCellSpawn;
        public static Func<int,int,Cell> OnRequestRandomCellSpawn;
        public static Action<Cell> OnCellDestroyed;
        public static Action<int,int,Cell> OnFilledCell;
        public static Action OnFillCompleted;
        public static Action OnFallCompleted;
        public static Action OnValidMatchCleared;
        public static Action<Cell,Cell> OnSwapCompleted;


    }
}
