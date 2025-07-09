using CellOverlays;
using Factories.GeneralFactories;
using Managers;
using Misc;
using UnityEngine;
namespace Spawners
{
    public class CellOverlaySpawner : MonoBehaviour
    {
        [SerializeField] private GeneralCellOverlayFactory cellOverlayFactory;
        public void OnEnable()
        {
            EventManager.RequestCellOverlaySpawn += SpawnCellOverlay;
        }

        private void OnDisable()
        {
            EventManager.RequestCellOverlaySpawn -= SpawnCellOverlay;
        }

 
        private BaseCellOverlay SpawnCellOverlay(CellOverlayType cellOverlayType, Vector3 position)
        {
            BaseCellOverlay cellOverlay = cellOverlayFactory.CreateCellOverlayBasedOnType(cellOverlayType);
            if (cellOverlay == null) return null;
            cellOverlay.transform.position = position;
            return cellOverlay;
        }
    }
}