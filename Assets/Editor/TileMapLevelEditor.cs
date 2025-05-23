#if UNITY_EDITOR
using System.Collections.Generic;
using Misc;
using TileRelated;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editor
{
    public class TilemapLevelEditor : EditorWindow
    {
        private TilemapLevelBaker _baker;
        private GridDataSo _gridData;

        public List<CellTile> cellTiles = new List<CellTile>();
        public List<PieceTile> pieceTiles = new List<PieceTile>();
        public List<TileBase> borderTiles = new List<TileBase>();

        [MenuItem("Tools/Tilemap Level Editor")]
        public static void Open()
        {
            GetWindow<TilemapLevelEditor>("Tilemap Level Editor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Tilemap Level Editor", EditorStyles.boldLabel);

            _baker = EditorGUILayout.ObjectField("Baker Target", _baker, typeof(TilemapLevelBaker), true) as TilemapLevelBaker;
            _gridData = EditorGUILayout.ObjectField("Grid Data SO", _gridData, typeof(GridDataSo), false) as GridDataSo;

            EditorGUILayout.Space();
            GUILayout.Label("Tile Lookup Tables", EditorStyles.boldLabel);

            SerializedObject so = new SerializedObject(this);
            SerializedProperty cellTilesProp = so.FindProperty("cellTiles");
            SerializedProperty pieceTilesProp = so.FindProperty("pieceTiles");
            SerializedProperty borderTilesProp = so.FindProperty("borderTiles");  // Add this line
            EditorGUILayout.PropertyField(cellTilesProp, new GUIContent("Cell Tiles"), true);
            EditorGUILayout.PropertyField(pieceTilesProp, new GUIContent("Piece Tiles"), true);
            EditorGUILayout.PropertyField(borderTilesProp, new GUIContent("Border Tiles"), true);  // Add this line
            so.ApplyModifiedProperties();

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Bake Tilemap to SO"))
            {
                if (_baker && _gridData)
                    BakeTilemap();
            }

            if (GUILayout.Button("Rebuild Tilemap from SO"))
            {
                if (_baker && _gridData)
                    RebuildTilemap();
            }
        }

        private void BakeTilemap()
        {
            var bounds = GetUsedTileBounds(_baker.cellTilemap);
            int rows = bounds.size.y;
            int cols = bounds.size.x;

            _gridData.rows = rows;
            _gridData.columns = cols;
            _gridData.tilemapOrigin = new Vector2Int(bounds.xMin, bounds.yMin);
            _gridData.bounds = bounds;
            var cellList = new List<GridDataSo.CellData>();
            var pieceList = new List<GridDataSo.PieceData>();
            var borderTileList = new List<BorderTileData>();
        
            // Process the tilemaps and add the tiles to the lists
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    int row = y - bounds.yMin;
                    int col = x - bounds.xMin;

                    Vector3Int pos = new Vector3Int(x, y, 0);

                    var cellTile = _baker.cellTilemap.GetTile(pos) as CellTile;
                    if (cellTile != null)
                    {
                        cellList.Add(new GridDataSo.CellData
                        {
                            row = row,
                            column = col,
                            cellType = cellTile.cellType
                        });
                    }
                    else
                    {
                        cellList.Add(new GridDataSo.CellData
                        {
                            row = row,
                            column = col,
                            cellType = CellType.Disabled,
                        });
                    }

                    var pieceTile = _baker.pieceTilemap.GetTile(pos) as PieceTile;
                    if (pieceTile != null)
                    {
                        pieceList.Add(new GridDataSo.PieceData
                        {
                            row = row,
                            column = col,
                            pieceType = pieceTile.pieceType
                        });
                    }

                    var borderTile = _baker.tileFrameTilemap.GetTile(pos);
                    if (borderTile != null)
                    {
                        borderTileList.Add(new BorderTileData
                        {
                            row = row,
                            column = col,
                            borderTile = borderTile 
                        });
                    }
                }
            }

            _gridData.cellDataArray = cellList.ToArray();
            _gridData.pieceDataArray = pieceList.ToArray();
            _gridData.borderTileDataArray = borderTileList.ToArray();  // Store border tile data

            EditorUtility.SetDirty(_gridData);
            Debug.Log("Baked Tilemap to SO");
        }

        private void RebuildTilemap()
        {
            var cellDict = new Dictionary<CellType, CellTile>();
            var pieceDict = new Dictionary<PieceType, PieceTile>();
            var borderTileDict = new Dictionary<string, TileBase>();  // Dictionary to store border tiles

            foreach (var ct in cellTiles)
                if (!cellDict.ContainsKey(ct.cellType)) cellDict[ct.cellType] = ct;

            foreach (var pt in pieceTiles)
                if (!pieceDict.ContainsKey(pt.pieceType)) pieceDict[pt.pieceType] = pt;

            foreach (var bt in borderTiles)
                if (!borderTileDict.ContainsKey(bt.name)) borderTileDict[bt.name] = bt; // Use the name or unique identifier for border tiles

            // Clear old tiles first
            _baker.cellTilemap.ClearAllTiles();
            _baker.pieceTilemap.ClearAllTiles();
            _baker.tileFrameTilemap.ClearAllTiles(); // Clear the border tilemap as well

            // Set the cell tiles
            foreach (var cell in _gridData.cellDataArray)
            {
                if (cellDict.TryGetValue(cell.cellType, out var tile))
                {
                    int x = cell.column + _gridData.tilemapOrigin.x;
                    int y = cell.row + _gridData.tilemapOrigin.y;
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    _baker.cellTilemap.SetTile(pos, tile);
                }
            }

            // Set the piece tiles
            foreach (var piece in _gridData.pieceDataArray)
            {
                if (pieceDict.TryGetValue(piece.pieceType, out var tile))
                {
                    int x = piece.column + _gridData.tilemapOrigin.x;
                    int y = piece.row + _gridData.tilemapOrigin.y;
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    _baker.pieceTilemap.SetTile(pos, tile);
                }
            }

            // Set the border tiles (tileframe)
            foreach (var borderTile in _gridData.borderTileDataArray)
            {
                if (borderTileDict.TryGetValue(borderTile.borderTile.name, out var tile))
                {
                    int x = borderTile.column + _gridData.tilemapOrigin.x;
                    int y = borderTile.row + _gridData.tilemapOrigin.y;
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    _baker.tileFrameTilemap.SetTile(pos, tile);
                }
            }

            Debug.Log(" Rebuilt Tilemaps from SO");
        }
        private BoundsInt GetUsedTileBounds(Tilemap tilemap)
        {
            var positions = new List<Vector3Int>();

            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(pos))
                {
                    positions.Add(pos);
                }
            }

            if (positions.Count == 0)
                return new BoundsInt();

            var min = positions[0];
            var max = positions[0];

            foreach (var pos in positions)
            {
                min = Vector3Int.Min(min, pos);
                max = Vector3Int.Max(max, pos);
            }

            // BoundsInt's size is inclusive of min and exclusive of max, so add 1
            var size = max - min + Vector3Int.one;
            return new BoundsInt(min, size);
        }
    
    }
}
#endif
