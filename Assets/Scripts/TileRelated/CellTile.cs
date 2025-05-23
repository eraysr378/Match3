using Misc;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileRelated
{
    [CreateAssetMenu(menuName = "TileRelated/CellTile")]
    public class CellTile : Tile
    {
        public CellType cellType;
    }
}