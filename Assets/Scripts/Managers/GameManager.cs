using System;
using System.Collections;
using Handlers;
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
        [SerializeField] private ScoreScreenUI scoreScreenUI;
        [SerializeField] private GridStabilizationChecker gridStabilizationChecker;
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
            scoreScreenUI.Show(scoreManager.GetTotalScore(), goalManager.IsAllGoalsCompleted());
        }
    }
}