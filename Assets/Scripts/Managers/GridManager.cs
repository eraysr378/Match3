using System.Collections.Generic;
using GridRelated;
using Interfaces;
using Pieces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }
        public Grid Grid { get; private set; }
        
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
            Grid = grid;
        }
        public List<T> GetPiecesInRadius<T>(int row, int col, int radius)
        {
            List<T> pieceOfTypeList = new();
            for (int r = row - radius; r <= row + radius; r++)
            {
                for (int c = col - radius; c <= col + radius; c++)
                {
                    Piece piece = Grid.GetCell(r, c)?.CurrentPiece;
                    if (piece != null && piece.TryGetComponent<T>(out var pieceOfType))
                    {
                        pieceOfTypeList.Add(pieceOfType);
                    }
                }
            }

            return pieceOfTypeList;
        }

 
        
    }
}