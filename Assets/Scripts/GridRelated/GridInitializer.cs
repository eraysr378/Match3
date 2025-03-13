using Cells;
using Factories;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace GridRelated
{
    public class GridInitializer : MonoBehaviour
    {
        [SerializeField] private GeneralCellFactory generalCellFactory;
        [SerializeField] private GridPropertiesSo propertiesSo;
        [SerializeField] private RectTransform playground;
        [SerializeField] private SpriteRenderer borderSpriteRenderer;
        [SerializeField] private Transform gridParent;

        public void Start()
        {
            Init();
            EventManager.OnGridInitialized?.Invoke(propertiesSo.grid);
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
            CreateGridElements();
        }
        private void AdjustGridSize( )
        {
            Vector3[] corners = new Vector3[4];
            playground.GetWorldCorners(corners);
            float gridPlaygroundWidth = corners[2].x - corners[0].x; // Top-right x - Bottom-left x
            float gridPlaygroundHeight = corners[1].y - corners[0].y; // Top-left y - Bottom-left y

            // Calculate the size for each element to fit exactly within the grid
            float elementWidth = gridPlaygroundWidth / propertiesSo.width;
            float elementHeight = gridPlaygroundHeight / propertiesSo.height;
            propertiesSo.elementSize = Mathf.Min(elementWidth, elementHeight); // Use the smaller size for square elements

            // Calculate the center of the playground
            propertiesSo.gridPlaygroundCenter = (corners[0] + corners[2]) / 2;

            // Calculate offsets to align the middle element with the center
            float totalGridWidth = propertiesSo.width * propertiesSo.elementSize;
            float totalGridHeight = propertiesSo.height * propertiesSo.elementSize;

            propertiesSo.gridOffset.x = -(totalGridWidth / 2) + (propertiesSo.elementSize / 2);
            propertiesSo.gridOffset.y = -(totalGridHeight / 2) + (propertiesSo.elementSize / 2);
            
            borderSpriteRenderer.size = new Vector2(totalGridWidth+propertiesSo.elementSize/8, totalGridHeight+propertiesSo.elementSize/8);
            borderSpriteRenderer.transform.position = new Vector3(propertiesSo.gridPlaygroundCenter.x, propertiesSo.gridPlaygroundCenter.y , borderSpriteRenderer.transform.position.z);
        }
        private void CreateGridElements()
        {
            if (propertiesSo.customGridSo != null)
            {
                for (int row = 0; row < propertiesSo.height; row++)
                {
                    for (int col = 0; col < propertiesSo.width; col++)
                    {
                        Cell createdCell = generalCellFactory.CreateCell(propertiesSo.customGridSo.customGrid.GetElement(row, col));
                        Vector2 pos = GridUtility.GridPositionToWorldPosition(row,col,createdCell
                            ,propertiesSo.gridPlaygroundCenter
                            ,propertiesSo.gridOffset
                            ,propertiesSo.elementSize);
                        createdCell.Init(row,col,pos,propertiesSo.elementSize,gridParent);
                        propertiesSo.grid.SetCell(row, col, createdCell);

                    }
                }
            }
            // else
            // {
            //     for (int row = 0; row < propertiesSo.height; row++)
            //     {
            //         for (int col = 0; col < propertiesSo.width; col++)
            //         {
            //             Cell createdCell = cellFactory.CreateCell(CellType.SquareNormalCell);
            //             Vector2 pos = GridUtility.GridPositionToWorldPosition(row,col,createdCell
            //                 ,propertiesSo.gridPlaygroundCenter
            //                 ,propertiesSo.gridOffset
            //                 ,propertiesSo.elementSize);
            //             createdCell.Init(row,col,pos,propertiesSo.elementSize,gridParent);
            //
            //             propertiesSo.grid.SetCell(row, col, createdCell);
            //
            //         }
            //     }
            // }
        }


    }
}
