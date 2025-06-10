using BuildSystem;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelDataSo currentLevelDataSo;
        [SerializeField] private GoalManager goalManager;
        [SerializeField] private MoveManager moveManager;
        [SerializeField] private GridBuilder gridBuilder;
        
        

        private void Start()
        {
            goalManager.InitializeGoals(currentLevelDataSo.goalConfigs);
            moveManager.SetTotalMoves(currentLevelDataSo.totalMoveCount);
            gridBuilder.Build(currentLevelDataSo.gridDataSo);
        }
    }
}