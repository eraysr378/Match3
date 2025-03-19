using System.Collections.Generic;
using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    public class GeneralCellFactory :MonoBehaviour
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
                if (!_cellFactoriesByType.TryAdd(_cellTypeKeys[i], _cellFactoryValues[i]))
                {
                    Debug.LogError("Duplicate key");
                }
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
    }
}