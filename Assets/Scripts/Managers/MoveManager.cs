using System;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class MoveManager : MonoBehaviour
    {
        [SerializeField] private int totalMoves = 5;
        [SerializeField] private TextMeshProUGUI moveText;
        
        public event Action OnOutOfMoves;
        
        private void Start()
        {
            UpdateMoveText();
        }

        public void MakeMove()
        {
            totalMoves--;
            UpdateMoveText();

            if (totalMoves == 0)
            {
                OnOutOfMoves?.Invoke();
            }
        }

        private void UpdateMoveText()
        {
            moveText.text = totalMoves.ToString();
        }

        public bool CanMakeMove()
        {
            return totalMoves > 0;
        }

        public void SetTotalMoves(int value)
        {
            totalMoves = value;
        }
    }
}