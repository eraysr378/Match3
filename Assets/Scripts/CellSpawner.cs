using UnityEngine;
using Cells;
using Factories;
using GridRelated;
using Managers;

public class CellSpawner : MonoBehaviour
{
    [SerializeField] private GeneralCellFactory cellFactory;
    [SerializeField] private Transform cellParent;

    public void OnEnable()
    {
        EventManager.OnRequestCellSpawn += SpawnCell;
        EventManager.OnRequestRandomCellSpawn += SpawnRandomNormalCell;
    }

    private void OnDisable()
    {
        EventManager.OnRequestCellSpawn -= SpawnCell;
        EventManager.OnRequestRandomCellSpawn -= SpawnRandomNormalCell;
    }

    private Cell SpawnCell(CellType cellType, int row, int col)
    {
        Cell newCell = cellFactory.CreateCellBasedOnType(cellType);
        if (newCell == null)
        {
            Debug.LogError($"Failed to create cell of type {cellType}");
            return null;
        }

        Vector2 pos = GridUtility.GridPositionToWorldPosition(row, col, newCell);
        newCell.Init(row, col, pos, GridUtility.PropertiesSo.elementSize, cellParent);
        return newCell;
    }

    private Cell SpawnRandomNormalCell(int row, int col)
    {
        Cell newCell = cellFactory.CreateRandomNormalCell();
        if (newCell == null)
        {
            Debug.LogError($"Failed to create random normal cell");
            return null;
        }

        Vector2 pos = GridUtility.GridPositionToWorldPosition(row, col, newCell);
        newCell.Init(row, col, pos, GridUtility.PropertiesSo.elementSize, cellParent);
        return newCell;
    }
}