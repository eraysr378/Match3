using System;
using System.Collections;
using Handlers;
using ScriptableObjects;
using UI;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private MoveManager moveManager;
        [SerializeField] private GoalManager goalManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private LevelCompletedScreenUI levelCompletedScreenUI;
        [SerializeField] private LevelFailedScreenUI levelFailedScreenUI;
        [SerializeField] private GridStabilizationChecker gridStabilizationChecker;
        [SerializeField] private LevelDataSo currentLevelDataSo;
        private bool _isGameOver;
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void OnEnable()
        {
            moveManager.OnOutOfMoves += OnGameOver;
            goalManager.OnAllGoalsCompleted += OnGameOver;
        }

        private void OnDisable()
        {
            moveManager.OnOutOfMoves -= OnGameOver;
            goalManager.OnAllGoalsCompleted -= OnGameOver;
        }

        private void OnGameOver()
        {
            if (_isGameOver)
            {
                return;
            }
            _isGameOver = true;
            inputHandler.gameObject.SetActive(false);
            StartCoroutine(HandleGameOverCoroutine());

        }

        private IEnumerator HandleGameOverCoroutine()
        {
            Debug.Log("Waiting for stabilization");
            yield return new WaitForSeconds(1);
            if (gridStabilizationChecker.IsGridStabilized() == false)
            {
                gridStabilizationChecker.OnGridStabilized += OnGridStabilized;
            }
            else
            {
                OnGridStabilized();
            }
        }

        private void OnGridStabilized()
        {
            gridStabilizationChecker.OnGridStabilized -= OnGridStabilized;
            if (goalManager.IsAllGoalsCompleted())
            {
                if (currentLevelDataSo.level == PlayerPrefs.GetInt("MaxLevel"))
                {
                    PlayerPrefs.SetInt("MaxLevel", currentLevelDataSo.level+1);
                }
                levelCompletedScreenUI.Show(scoreManager.GetTotalScore());
            }
            else
            {
                levelFailedScreenUI.Show(scoreManager.GetTotalScore());
            }
        }
    }
}