using System;
using Cells;
using GridRelated;

namespace Managers
{
    public static class EventManager
    {
        public static Action<Cell> OnPointerDownCell;
        public static Action<Cell> OnPointerUpCell;
        public static Action<Cell> OnPointerEnterCell;
        public static Action<Cell> OnPointerClickedCell;
        public static Action<Grid> OnGridInitialized;
        
    
    }
}
