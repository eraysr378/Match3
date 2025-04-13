using System.Collections.Generic;
using Cells;
using GridRelated;
using Interfaces;
using Misc;
using Pieces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }
        public int Height => _grid.Height;
        public int Width => _grid.Width;
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
            GridInitializer.OnGridInitialized += OnGridInitialized;
        }

        private void OnDisable()
        {
            GridInitializer.OnGridInitialized -= OnGridInitialized;
        }

        private void OnGridInitialized(Grid grid)
        {
            _grid = grid;
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
        public List<Cell> GetCellsInRadius(int row, int col, int radius)
        {
            List<Cell> cellList = new();
            for (int r = row - radius; r <= row + radius; r++)
            {
                for (int c = col - radius; c <= col + radius; c++)
                {
                    Cell cell = _grid.GetCellAt(r, c);
                    if (cell != null)
                    {
                        cellList.Add(cell);
                    }
                }
            }

            return cellList;
        }
        public List<T> GetAll<T>()
        {
            List<T> pieceOfTypeList = new();
            for (int r = 0; r <= _grid.Height; r++)
            {
                for (int c = 0; c <= _grid.Width; c++)
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

        public List<Cell> GetCellsInRow(int row)
        {
            List<Cell> cellList = new();
            for (int col = 0; col < _grid.Width; col++)
            {
                Cell cell = _grid.GetCellAt(row, col);
                cellList.Add(cell);
            }
            return cellList;
        }
        public List<Cell> GetCellsInCol(int col)
        {
            List<Cell> cellList = new();
            for (int row = 0; row < _grid.Height; row++)
            {
                Cell cell = _grid.GetCellAt(row, col);
                cellList.Add(cell);
            }
            return cellList;
        }
        public List<Cell> GetCellsBelow(int row,int col)
        {
            List<Cell> cellList = new();
            for (int r = row-1; r >= 0; r--)
            {
                Cell cell = _grid.GetCellAt(r, col);
                cellList.Add(cell);
            }
            return cellList;
        }

        public List<Cell> GetCellsAbove(int row, int col)
        {
            List<Cell> cellList = new();
            for (int r = row + 1; r < _grid.Height; r++)
            {
                Cell cell = _grid.GetCellAt(r, col);
                cellList.Add(cell);
            }

            return cellList;
        }
        public List<Cell> GetCellsRight(int row, int col)
        {
            List<Cell> cellList = new();
            for (int c = col + 1; c < _grid.Width; c++)
            {
                Cell cell = _grid.GetCellAt(row, c);
                cellList.Add(cell);
            }

            return cellList;
        }
        public List<Cell> GetCellsLeft(int row, int col)
        {
            List<Cell> cellList = new();
            for (int c = col -1; c >= 0; c--)
            {
                Cell cell = _grid.GetCellAt(row, c);
                cellList.Add(cell);
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
                    if(piece?.GetPieceType() == pieceType)
                        return piece;
                }
            }
            return null;
        }

        public Cell GetCellAt(int row, int col)
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
                    if(piece != null)
                        pieces.Add(piece);
                    
                }
            }
            return pieces;
        }
        public List<Cell> GetAllCells()
        {
            List<Cell> cells = new();
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
 
        
    }
}