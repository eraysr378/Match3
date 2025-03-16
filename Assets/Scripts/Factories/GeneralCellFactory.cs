using System;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Misc;
using UnityEngine;

namespace Factories
{
    public class GeneralCellFactory : MonoBehaviour
    {
        [SerializeField] private List<CellType> _cellTypeKeys;
        [SerializeField] private List<CellFactory> _cellFactoryValues;
        private Dictionary<CellType, CellFactory> _cellFactoriesByType;
        
        private void Awake()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            _cellFactoriesByType = new Dictionary<CellType, CellFactory>();
            for (int i = 0; i < Mathf.Min(_cellTypeKeys.Count, _cellFactoryValues.Count); i++)
            {
                _cellFactoriesByType.Add(_cellTypeKeys[i], _cellFactoryValues[i]);
            }
        }

        public Cell CreateCellBasedOnType(CellType cellType)
        {
            if (_cellFactoriesByType.TryGetValue(cellType, out var factory))
            {
                return factory.CreateCell(cellType);
            }
            return null;
        }

        // public Cell CreateRandomNormalCell()
        // {
        //     // Filter out only the CellTypes that are created by NormalCellFactory
        //     List<CellType> normalCellTypes = _cellFactoriesByType
        //         .Where(kvp => kvp.Value is NormalCellFactory) 
        //         .Select(kvp => kvp.Key) 
        //         .ToList();
        //
        //     if (normalCellTypes.Count == 0)
        //         throw new Exception("No NormalCellFactory found in the dictionary.");
        //
        //     CellType randomCellType = normalCellTypes[UnityEngine.Random.Range(0, normalCellTypes.Count)];
        //
        //     // Use the factory to create the cell
        //     return CreateCellBasedOnType(randomCellType);
        // }
  
    }
}
