using Combinations;
using Factories.CombinationFactories;
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
            EventManager.OnCombinationSpawnRequested += SpawnCombination;
        }

        private void OnDisable()
        {
            EventManager.OnCombinationSpawnRequested -= SpawnCombination;
        }

 
        private Combination SpawnCombination(CombinationType combinationType, int row, int col)
        {
            Combination combination = combinationFactory.CreateCombinationBasedOnType(combinationType);
            if (combination == null) return null;
            
            Vector2 pos = GridUtility.GridPositionToWorldPosition(row, col);
            
            combination.transform.position = pos;
            
            return combination;
        }
    }
}