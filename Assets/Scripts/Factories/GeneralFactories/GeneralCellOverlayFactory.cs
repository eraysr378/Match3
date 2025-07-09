using System.Collections.Generic;
using CellOverlays;
using Cells;
using Factories.CellFactories;
using Factories.CellOverlayFactories;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

namespace Factories.GeneralFactories
{
    public class GeneralCellOverlayFactory : MonoBehaviour
    {
        [SerializeField] private Transform cellOverlayParent;
        [SerializeField] private List<CellOverlayType> cellOverlayTypeKeys;
        [SerializeField] private List<BaseCellOverlayFactory> cellOverlayFactoryValues;
        private Dictionary<CellOverlayType, BaseCellOverlayFactory> _cellFactoriesByType;
        
        private void Awake()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            _cellFactoriesByType = new Dictionary<CellOverlayType, BaseCellOverlayFactory>();
            for (int i = 0; i < Mathf.Min(cellOverlayTypeKeys.Count, cellOverlayFactoryValues.Count); i++)
            {
                if (!_cellFactoriesByType.TryAdd(cellOverlayTypeKeys[i], cellOverlayFactoryValues[i]))
                {
                    Debug.LogError("Duplicate key");
                }
            }
        }

        public BaseCellOverlay CreateCellOverlayBasedOnType(CellOverlayType cellOverlayType)
        {
            if (_cellFactoriesByType.TryGetValue(cellOverlayType, out var factory))
            {
                return factory.CreateCellOverlay(cellOverlayType, cellOverlayParent);
            }
            return null;
        }
    }
}