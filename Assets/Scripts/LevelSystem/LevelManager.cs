using Managers;
using ScriptableObjects;
using TileRelated;
using UnityEngine;
namespace LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelDataSo currentLevelDataSo;
        [SerializeField] private GoalManager goalManager;
        [SerializeField] private MoveManager moveManager;
        [SerializeField] private TilemapLoader tilemapLoader;
        private void Start()
        {
            goalManager.InitializeGoals(currentLevelDataSo.goalConfigs);
            moveManager.SetTotalMoves(currentLevelDataSo.totalMoveCount);
            tilemapLoader.Load(currentLevelDataSo.gridDataSo);
        }
    }
}