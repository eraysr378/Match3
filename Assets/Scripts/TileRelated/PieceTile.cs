using Misc;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileRelated
{
    [CreateAssetMenu(menuName = "TileRelated/PieceTile")]
    public class PieceTile : Tile
    {
        public PieceType pieceType;
    }
}