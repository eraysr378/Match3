using System;
using CameraRelated;
using TileRelated;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildSystem
{
    public class GridBuilder : MonoBehaviour
    {
        public Tilemap borderTilemap;
        public static event Action<GridRelated.Grid> OnCellsCreated;
        private GridDataSo _gridData;
        private GridRelated.Grid _grid;
        private CellBuilder _cellBuilder;
        private PieceBuilder _pieceBuilder;
        private CellOverlayBuilder _cellOverlayBuilder;


        private void Awake()
        {
            _cellBuilder = new CellBuilder();
            _pieceBuilder = new PieceBuilder();
            _cellOverlayBuilder = new CellOverlayBuilder();
        }

        public void Build(GridDataSo gridData)
        {
            _gridData = gridData;
            if (gridData == null)
            {
                Debug.LogError("GridDataSo is not assigned!");
                return;
            }
            int rows = gridData.rows;
            int columns = gridData.columns;
            _grid = new GridRelated.Grid(rows, columns);

            _cellBuilder.Build(gridData,_grid);
            OnCellsCreated?.Invoke(_grid);
            _cellOverlayBuilder.Build(gridData, _grid);
            _pieceBuilder.Build(gridData, _grid);
            CreateBorderTiles();
            CameraInitializer.ComputeCameraView(_grid.Height,_grid.Width);
        }
        private void CreateBorderTiles()
        {
            foreach (var borderTileData in _gridData.borderTileDataArray)
            {
                Vector3Int tilePosition = new Vector3Int(borderTileData.column, borderTileData.row, 0);
                borderTilemap.SetTile(tilePosition, borderTileData.borderTile);
            }
        }
    }
}
