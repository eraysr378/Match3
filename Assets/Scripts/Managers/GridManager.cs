using System.Collections.Generic;
using BuildSystem;
using Cells;
using GridRelated;
using Interfaces;
using Misc;
using Pieces;
using TileRelated;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }
        public int Height => _grid.Height;
        public int Width => _grid.Width;
        public Vector3 GridOrigin { get; private set; }
        private Grid _grid;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            GridBuilder.OnCellsCreated += OnGridInitialized;
        }

        private void OnDisable()
        {
            GridBuilder.OnCellsCreated -= OnGridInitialized;
        }

        private void OnGridInitialized(Grid grid)
        {
            _grid = grid;
            CalculateGridOrigin();
            // Debug.Log(GridOrigin);
        }

        // Find the position of cell in (0,0) we cant use the cell directly because it might be null
        private void CalculateGridOrigin()
        {
            for (int r = 0; r <= _grid.Height; r++)
            {
                for (int c = 0; c <= _grid.Width; c++)
                {
                    var cell = _grid.GetCellAt(r, c);
                    if (cell != null)
                    {
                        GridOrigin = cell.transform.position - new Vector3(c, r, 0);
                        return;
                    }
                }
            }
        }

        public List<T> GetPiecesInRadius<T>(int row, int col, int radius)
        {
            List<T> pieceOfTypeList = new();
            for (int r = row - radius; r <= row + radius; r++)
            {
                for (int c = col - radius; c <= col + radius; c++)
                {
                    Piece piece = _grid.GetCellAt(r, c)?.CurrentPiece;
                    if (piece != null && piece.TryGetComponent<T>(out var pieceOfType))
                    {
                        pieceOfTypeList.Add(pieceOfType);
                    }
                }
            }
            return pieceOfTypeList;
        }

        public List<BaseCell> GetCellsInRadius(int row, int col, int radius)
        {
            List<BaseCell> cellList = new();
            for (int r = row - radius; r <= row + radius; r++)
            {
                for (int c = col - radius; c <= col + radius; c++)
                {
                    BaseCell cell = _grid.GetCellAt(r, c);
                    if (cell != null)
                    {
                        cellList.Add(cell);
                    }
                }
            }

            return cellList;
        }
        
        public List<BaseCell> GetCellsInRow(int row)
        {
            List<BaseCell> cellList = new();
            for (int col = 0; col < _grid.Width; col++)
            {
                BaseCell cell = _grid.GetCellAt(row, col);
                if (cell != null)
                {
                    cellList.Add(cell);
                }
            }

            return cellList;
        }

        public List<BaseCell> GetCellsInCol(int col)
        {
            List<BaseCell> cellList = new();
            for (int row = 0; row < _grid.Height; row++)
            {
                BaseCell cell = _grid.GetCellAt(row, col);
                if (cell != null)
                {
                    cellList.Add(cell);
                }
            }

            return cellList;
        }

        public List<BaseCell> GetCellsBelow(int row, int col)
        {
            List<BaseCell> cellList = new();
            for (int r = row - 1; r >= 0; r--)
            {
                BaseCell cell = _grid.GetCellAt(r, col);
                if (cell != null)
                {
                    cellList.Add(cell);
                }
            }

            return cellList;
        }

        public List<BaseCell> GetCellsAbove(int row, int col)
        {
            List<BaseCell> cellList = new();
            for (int r = row + 1; r < _grid.Height; r++)
            {
                BaseCell cell = _grid.GetCellAt(r, col);
                if (cell != null)
                {
                    cellList.Add(cell);
                }
            }

            return cellList;
        }

        public List<BaseCell> GetCellsRight(int row, int col)
        {
            List<BaseCell> cellList = new();
            for (int c = col + 1; c < _grid.Width; c++)
            {
                BaseCell cell = _grid.GetCellAt(row, c);
                if (cell != null)
                {
                    cellList.Add(cell);
                }
            }

            return cellList;
        }

        public List<BaseCell> GetCellsLeft(int row, int col)
        {
            List<BaseCell> cellList = new();
            for (int c = col - 1; c >= 0; c--)
            {
                BaseCell cell = _grid.GetCellAt(row, c);
                if (cell != null)
                {
                    cellList.Add(cell);
                }
            }

            return cellList;
        }

        public Piece GetPieceOfType(PieceType pieceType)
        {
            for (int r = 0; r <= _grid.Height; r++)
            {
                for (int c = 0; c <= _grid.Width; c++)
                {
                    Piece piece = _grid.GetCellAt(r, c)?.CurrentPiece;
                    if (piece?.GetPieceType() == pieceType)
                        return piece;
                }
            }

            return null;
        }

        public BaseCell GetCellAt(int row, int col)
        {
            return _grid.GetCellAt(row, col);
        }

        public List<Piece> GetAllPieces()
        {
            List<Piece> pieces = new();
            for (int row = 0; row < _grid.Height; row++)
            {
                for (int col = 0; col < _grid.Width; col++)
                {
                    Piece piece = GetCellAt(row, col).CurrentPiece;
                    if (piece != null)
                        pieces.Add(piece);
                }
            }

            return pieces;
        }

        public List<BaseCell> GetAllCells()
        {
            List<BaseCell> cells = new();
            for (int row = 0; row < _grid.Height; row++)
            {
                for (int col = 0; col < _grid.Width; col++)
                {
                    cells.Add(_grid.GetCellAt(row, col));
                }
            }

            return cells;
        }

        public List<Piece> GetPiecesByType(PieceType type)
        {
            List<Piece> piecesOfType = new List<Piece>();

            for (int row = 0; row < _grid.Height; row++)
            {
                for (int col = 0; col < _grid.Width; col++)
                {
                    Piece piece = _grid.GetCellAt(row, col)?.CurrentPiece;
                    if (piece != null && piece.GetPieceType() == type)
                    {
                        piecesOfType.Add(piece);
                    }
                }
            }

            return piecesOfType;
        }

        public List<Piece> GetPiecesInRow(int row)
        {
            List<Piece> piecesInRow = new List<Piece>();
            for (int col = 0; col < _grid.Width; col++)
            {
                Piece piece = _grid.GetCellAt(row, col)?.CurrentPiece;
                if (piece != null)
                {
                    piecesInRow.Add(piece);
                }
            }

            return piecesInRow;
        }

        public List<BaseCell> GetAdjacentCells(int row, int col)
        {
            List<BaseCell> cells = new();
            
            var cellLeft = _grid.GetCellAt(row, col - 1);
            var cellRight = _grid.GetCellAt(row, col + 1);
            var cellBelow = _grid.GetCellAt(row-1, col);
            var cellAbove = _grid.GetCellAt(row+1, col);
            if (cellLeft != null)
            {
                cells.Add(cellLeft);
            }
            if (cellRight != null)
            {
                cells.Add(cellRight);
            }
            if (cellBelow != null)
            {
                cells.Add(cellBelow);
            }
            if (cellAbove != null)
            {
                cells.Add(cellAbove);
            }

            return cells;

        }
        public bool AreAllCellsFilled()
        {
            for (int row = 0; row < _grid.Height; row++)
            {
                for (int col = 0; col < _grid.Width; col++)
                {
                    if (_grid.GetCellAt(row, col) == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}