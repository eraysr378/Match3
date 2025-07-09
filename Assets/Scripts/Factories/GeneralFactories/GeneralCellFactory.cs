using System.Collections.Generic;
using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    public class GeneralCellFactory :MonoBehaviour
    {
        [SerializeField] private Transform cellParent;
        [SerializeField] private List<CellType> _cellTypeKeys;
        [SerializeField] private List<BaseCellFactory> _cellFactoryValues;
        private Dictionary<CellType, BaseCellFactory> _cellFactoriesByType;
        
        private void Awake()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            _cellFactoriesByType = new Dictionary<CellType, BaseCellFactory>();
            for (int i = 0; i < Mathf.Min(_cellTypeKeys.Count, _cellFactoryValues.Count); i++)
            {
                if (!_cellFactoriesByType.TryAdd(_cellTypeKeys[i], _cellFactoryValues[i]))
                {
                    Debug.LogError("Duplicate key");
                }
            }
        }

        public BaseCell CreateCellBasedOnType(CellType cellType)
        {
            if (_cellFactoriesByType.TryGetValue(cellType, out var factory))
            {
                return factory.CreateCell(cellType,cellParent);
            }
            return null;
        }
    }
}