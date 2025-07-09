using BuildSystem;
using Database;
using Managers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelDataSo currentLevelDataSo;
        [SerializeField] private GoalManager goalManager;
        [SerializeField] private MoveManager moveManager;
        [SerializeField] private GridBuilder gridBuilder;

        private void OnEnable()
        {
            EventManager.OnNextLevelSelected += LoadNextLevel;
            EventManager.OnCurrentLevelSelected += LoadCurrentLevel;
        }

        private void OnDisable()
        {
            EventManager.OnNextLevelSelected -= LoadNextLevel;
            EventManager.OnCurrentLevelSelected -= LoadCurrentLevel;
        }

        private void Start()
        {
            goalManager.InitializeGoals(currentLevelDataSo.goalConfigs);
            moveManager.SetTotalMoves(currentLevelDataSo.totalMoveCount);
            gridBuilder.Build(currentLevelDataSo.gridDataSo);
        }

        private void LoadNextLevel()
        {
            var nextLevelData = LevelDatabase.Instance.GetLevelDataSo(currentLevelDataSo.level + 1);
            currentLevelDataSo.SetFrom(nextLevelData);
            SceneManager.LoadScene("GameScene");
        }

        private void LoadCurrentLevel()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}