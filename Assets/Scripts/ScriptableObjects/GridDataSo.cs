using UnityEngine;
using Cells;
using Misc;
using Pieces;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[System.Serializable]
public class BorderTileData
{
    public int row;
    public int column;
    public TileBase borderTile;
}
[CreateAssetMenu(fileName = "GridDataLevel",menuName = "ScriptableObjects/Grid Data")]
public class GridDataSo : ScriptableObject
{
    public int rows;
    public int columns;
    public Vector2Int tilemapOrigin;
    public BoundsInt bounds;
    public BorderTileData[] borderTileDataArray; 
    public CellData[] cellDataArray;
    public PieceData[] pieceDataArray;
    public CellOverlayData[] cellOverlayDataArray;
    [System.Serializable]
    public struct CellData
    {
        public int row;
        public int column;
        public CellType cellType;
    }

    [System.Serializable]
    public struct PieceData
    {
        public int row;
        public int column;
        public PieceType pieceType;
    }
    [System.Serializable]

    public struct CellOverlayData
    {
        public int row;
        public int column;
        public CellOverlayType overlayType;
    }


}