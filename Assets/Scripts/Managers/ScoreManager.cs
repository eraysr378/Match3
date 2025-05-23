using System;
using System.Collections.Generic;
using Pieces;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int scorePerPiece = 1;
        [SerializeField] private int totalScore;
        [SerializeField] private  TextMeshProUGUI scoreText;
        private void OnEnable()
        {
            EventManager.OnPieceScored += OnPieceScored;
        }
        private void OnDisable()
        {
            EventManager.OnPieceScored -= OnPieceScored;
        }

        private void OnPieceScored(Piece piece)
        {
            UpdateTotalScore(scorePerPiece);
        }

        private void UpdateTotalScore(int value)
        {
            totalScore += value;
            scoreText.text = totalScore.ToString();

        }

        public int GetTotalScore()
        {
            return totalScore;
        }
        
    }
}