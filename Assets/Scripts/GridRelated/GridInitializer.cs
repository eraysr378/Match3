using System;
using Cells;
using Pieces;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace GridRelated
{
    public class GridInitializer : MonoBehaviour
    {
        public static event Action<Grid> OnGridInitialized;

        [SerializeField] private GridPropertiesSo propertiesSo;
        [SerializeField] private RectTransform playground;
        [SerializeField] private SpriteRenderer borderSpriteRenderer;

        public void Awake()
        {
            GridUtility.PropertiesSo = propertiesSo;
        }

        public void Start()
        {
            Init();
            OnGridInitialized?.Invoke(propertiesSo.grid);
            CreateGridPieces();
        }

        private void Init()
        {
            if (propertiesSo.customGridSo != null)
            {
                propertiesSo.height = propertiesSo.customGridSo.customGrid.height;
                propertiesSo.width = propertiesSo.customGridSo.customGrid.width;
            }

            propertiesSo.grid = new Grid(propertiesSo.height, propertiesSo.width);
            AdjustGridSize();
            CreateGridCells();
        }
        private void AdjustGridSize()
        {
            Vector3[] corners = new Vector3[4];
            playground.GetWorldCorners(corners);
            float gridPlaygroundWidth = corners[2].x - corners[0].x; // Top-right x - Bottom-left x
            float gridPlaygroundHeight = corners[1].y - corners[0].y; // Top-left y - Bottom-left y

            float spacingRatio = propertiesSo.spacingRatio;

            // Calculate available space considering spacing
            float totalSpacingX = (propertiesSo.width - 1) * spacingRatio;
            float totalSpacingY = (propertiesSo.height - 1) * spacingRatio;
    
            float elementWidth = (gridPlaygroundWidth - totalSpacingX) / propertiesSo.width;
            float elementHeight = (gridPlaygroundHeight - totalSpacingY) / propertiesSo.height;
    
            propertiesSo.cellSize = Mathf.Min(elementWidth, elementHeight); // Use the smaller size

            // Calculate the total width and height of the grid including spacing
            float totalGridWidth = (propertiesSo.width * propertiesSo.cellSize) + totalSpacingX;
            float totalGridHeight = (propertiesSo.height * propertiesSo.cellSize) + totalSpacingY;

            // Calculate the center of the playground
            propertiesSo.gridPlaygroundCenter = (corners[0] + corners[2]) / 2;

            // Calculate offsets to align the middle element with the center
            propertiesSo.gridOffset.x = -(totalGridWidth / 2) + (propertiesSo.cellSize / 2);
            propertiesSo.gridOffset.y = -(totalGridHeight / 2) + (propertiesSo.cellSize / 2);

            // Adjust border size to fit new grid dimensions
            borderSpriteRenderer.size = new Vector2(totalGridWidth + propertiesSo.cellSize / 8, totalGridHeight + propertiesSo.cellSize / 8);
            borderSpriteRenderer.transform.position = new Vector3(propertiesSo.gridPlaygroundCenter.x, propertiesSo.gridPlaygroundCenter.y, borderSpriteRenderer.transform.position.z);
        }
        private void CreateGridCells()
        {
            if (propertiesSo.customGridSo != null)
            {
                for (int row = 0; row < propertiesSo.height; row++)
                {
                    for (int col = 0; col < propertiesSo.width; col++)
                    {
                        CustomGridCell customCell = propertiesSo.customGridSo.customGrid.GetElement(row, col);
                        
                        Cell createdCell = EventManager.OnCellSpawnRequested?.Invoke(customCell.cellType, row, col);
                        
                        propertiesSo.grid.SetCellAt(row, col, createdCell);

                    }
                }
            }
        }

        private void CreateGridPieces()
        {
            if (propertiesSo.customGridSo != null)
            {
                for (int row = 0; row < propertiesSo.height; row++)
                {
                    for (int col = 0; col < propertiesSo.width; col++)
                    {
                        CustomGridCell customCell = propertiesSo.customGridSo.customGrid.GetElement(row, col);
                        
                        Piece createdPiece = EventManager.OnPieceSpawnRequested?.Invoke(customCell.pieceType,row, col);
                        Cell cell = propertiesSo.grid.GetCellAt(row, col);
                        cell.SetPiece(createdPiece);
                        createdPiece?.Init(cell);

                    }
                }
            }
        }
        
    }
}
