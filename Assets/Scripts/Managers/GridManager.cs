using GridRelated;
using Pieces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }
        public Grid Grid { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        private void OnEnable()
        {
            GridInitializer.OnGridInitialized += OnGridInitialized;
        }

        private void OnDisable()
        {
            GridInitializer.OnGridInitialized -= OnGridInitialized;
        }

        private void OnGridInitialized(Grid grid)
        {
            Grid = grid;
        }

 
        
    }
}