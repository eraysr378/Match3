using System;
using Misc;
using UnityEngine;

namespace Pieces.CombinationPieces
{
    public abstract class Combination : MonoBehaviour
    {
        public event Action OnCombinationCompleted;  
        protected CombinationType combinationType;

        public abstract void ExecuteEffect(int row,int col);

        protected virtual void CompleteCombination()
        {
            OnCombinationCompleted?.Invoke();
        }

        public CombinationType GetCombinationType()
        {
            return combinationType;
        }
        
        
    }
}