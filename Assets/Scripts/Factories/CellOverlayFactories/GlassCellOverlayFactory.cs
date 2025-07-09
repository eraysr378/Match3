using CellOverlays;
using Misc;
using UnityEngine;

namespace Factories.CellOverlayFactories
{
    [CreateAssetMenu(fileName = "GlassCellOverlayFactory", menuName = "Factories/CellOverlay/GlassCellOverlayFactory")]

    public class GlassCellOverlayFactory : BaseCellOverlayFactory
    {
        [SerializeField] private GlassCellOverlay prefab;

        public override BaseCellOverlay CreateCellOverlay(CellOverlayType cellOverlayType,Transform parent = null)
        {
            GlassCellOverlay cellOverlay = Instantiate(prefab,parent);
            return cellOverlay;
        }
    }
}