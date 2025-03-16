using System;
using System.Collections.Generic;
using UnityEngine;
using Cells;
using Factories;
using GridRelated;
using Managers;
using Misc;
using Random = System.Random;

public class CellSpawner : MonoBehaviour
{
    private readonly Dictionary<CellType, Queue<Cell>> _cellPools = new();
    [SerializeField] private GeneralCellFactory cellFactory;
    [SerializeField] private Transform cellParent;

    public void OnEnable()
    {
        EventManager.OnRequestCellSpawn += SpawnCell;
        EventManager.OnRequestRandomNormalCellSpawn += SpawnRandomNormalCell;
        EventManager.OnCellReturnToPool += ReturnCellToPool;
    }

    private void OnDisable()
    {
        EventManager.OnRequestCellSpawn -= SpawnCell;
        EventManager.OnRequestRandomNormalCellSpawn -= SpawnRandomNormalCell;
        EventManager.OnCellReturnToPool -= ReturnCellToPool;
    }

 
    private Cell SpawnCell(CellType cellType, int row, int col)
    {
        Cell cell;
        if (_cellPools.TryGetValue(cellType, out Queue<Cell> pool) && pool.Count > 0)
        {
            cell = pool.Dequeue();
            cell.gameObject.SetActive(true);
            cell.OnSpawn();
        }
        else
        {
            cell = cellFactory.CreateCellBasedOnType(cellType);
        }
        Vector2 pos = GridUtility.GridPositionToWorldPosition(row, col, cell);
        cell.Init(row, col, pos, GridUtility.PropertiesSo.elementSize, cellParent);
        return cell;
    }

    private void ReturnCellToPool(Cell cell)
    {
        if (!_cellPools.ContainsKey(cell.GetCellType()))
        {
            _cellPools[cell.GetCellType()] = new Queue<Cell>();
        }

        _cellPools[cell.GetCellType()].Enqueue(cell);
    }
    private Cell SpawnRandomNormalCell(int row, int col)
    {
        Array values = Enum.GetValues(typeof(NormalCellType));
        var random = new Random();
        NormalCellType randomNormalCell = (NormalCellType)values.GetValue(random.Next(values.Length));
        CellType randomCellType = (CellType)randomNormalCell;
        
        return SpawnCell(randomCellType, row, col);

    }
}