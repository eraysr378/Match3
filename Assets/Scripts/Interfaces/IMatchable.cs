using System;
using System.Collections.Generic;
using Cells;
using Pieces;

namespace Interfaces
{
    public interface IMatchable
    {
        public event Action OnMatchHandled;

        public bool TryHandleNormalMatch(Action onHandled);
        public bool TryHandleSpecialMatch(Cell spawnCell,Action onHandled);
    }
}