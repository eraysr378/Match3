using Misc;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileRelated
{
    [CreateAssetMenu(menuName = "TileRelated/CellOverlayTile")]

    public class CellOverlayTile : Tile
    {
        public CellOverlayType cellOverlayType;

    }
}