using System;
using Cells;
using Managers;
using Misc;
using Pieces;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileRelated
{
    public class TilemapLoader : MonoBehaviour
    {
        public GridPropertiesSo propertiesSo;
        public Tilemap borderTilemap;


        public static event Action<GridRelated.Grid> OnCellsCreated;
        private GridDataSo _gridData;
        

        public void Load(GridDataSo gridData)
        {
            _gridData = gridData;
            if (gridData == null)
            {
                Debug.LogError("LevelDataSO is not assigned!");
                return;
            }

            // Create a new grid with dimensions from the LevelDataSO
            int rows = gridData.rows;
            int columns = gridData.columns;
            propertiesSo.grid = new GridRelated.Grid(rows, columns);

            CreateCells();
            OnCellsCreated?.Invoke(propertiesSo.grid);
            CreatePieces();
            CreateBorder();
            ComputeCameraView();
        }
        private void CreateBorder()
        {
            foreach (var borderTileData in _gridData.borderTileDataArray)
            {
                Vector3Int tilePosition = new Vector3Int(borderTileData.column, borderTileData.row, 0);
                borderTilemap.SetTile(tilePosition, borderTileData.borderTile);
            }
        }
        private void CreateCells()
        {
            foreach (var cellData in _gridData.cellDataArray)
            {
                // Instantiate the cell using the data from the LevelDataSO
                CellType cellType = cellData.cellType;
                Vector3 position = new Vector3(cellData.column + 0.5f, cellData.row + 0.5f, 0);
                
                // Create the cell
                Cell createdCell = EventManager.OnCellSpawnRequested?.Invoke(cellType, position);
                createdCell.SetPosition(cellData.row, cellData.column);

                // Place the cell into the grid
                propertiesSo.grid.SetCellAt(cellData.row, cellData.column, createdCell);
            }
        }

        private void CreatePieces()
        {
            foreach (var pieceData in _gridData.pieceDataArray)
            {
                // Instantiate the piece using the data from the LevelDataSO
                PieceType pieceType = pieceData.pieceType;
                int row = pieceData.row;
                int col = pieceData.column;

                // Create the piece
                Piece createdPiece = EventManager.OnPieceSpawnRequested?.Invoke(pieceType, row, col);
                Cell cell = propertiesSo.grid.GetCellAt(row, col);

                // Attach the piece to the corresponding cell
                cell.SetPiece(createdPiece);
                createdPiece?.Init(cell);
            }
        }
        private void ComputeCameraView()
        {
            BoundsInt bounds = _gridData.bounds;

            Vector3 center = new Vector3((float)_gridData.columns / 2, (float)_gridData.rows / 2, 0);
            Camera.main.transform.position = center + Vector3.back * 10.0f + Vector3.up * 0.75f;

            float halfSize = 0.0f;
            
            if (Screen.height > Screen.width)
            {
                float screenRatio = Screen.height / (float)Screen.width;
                halfSize = ((_gridData.columns + 1) * 0.5f) * screenRatio;
            }
            else
            {
                //On Wide screen, we fit vertically
                halfSize = (_gridData.rows + 3) * 0.5f ;
            }
            Camera.main.orthographicSize = halfSize;
        }
    }
}
