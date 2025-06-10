using CellOverlays;
using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellOverlayFactories
{
    public abstract class BaseCellOverlayFactory : ScriptableObject
    {
        public abstract BaseCellOverlay CreateCellOverlay(CellOverlayType cellOverlayType);

    }
}