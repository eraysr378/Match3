using System;
using System.Collections.Generic;
using Cells;
using Pieces;

namespace Interfaces
{
    public interface IMatchable
    {
        public event Action OnMatchHandled;

        public void OnNormalMatch();
        public void OnSpecialMatch(Cell spawnCell);
    }
}