using Cells;
using Combinations;
using Factories.CombinationFactories;
using Factories.GeneralFactories;
using GridRelated;
using Managers;
using Misc;
using UnityEngine;

namespace Spawners
{
    public class CombinationSpawner : MonoBehaviour
    {
        [SerializeField] private GeneralCombinationFactory combinationFactory;
        public void OnEnable()
        {
            EventManager.RequestCombinationSpawn += SpawnCombination;
        }

        private void OnDisable()
        {
            EventManager.RequestCombinationSpawn -= SpawnCombination;
        }

 
        private BaseCombination SpawnCombination(CombinationType combinationType, int row, int col)
        {
            BaseCombination combination = combinationFactory.GetCombinationBasedOnType(combinationType);
            if (combination == null) return null;
            
            // Vector2 pos = GridUtility.GridPositionToWorldPosition(row, col);
            BaseCell baseCell = GridManager.Instance.GetCellAt(row, col);
            combination.transform.position = baseCell.transform.position;
            
            return combination;
        }
    }
}