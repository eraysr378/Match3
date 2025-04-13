using System;
using Cells;
using Combinations;
using CombinationSystem;
using Interfaces;
using Misc;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;

namespace Managers
{
    public class CombinationManager : MonoBehaviour
    {
        public static event Action OnCombinationStarted;
        public static event Action OnCombinationCompleted;
        private readonly CombinationRuleSet _ruleSet = new CombinationRuleSet();
        private BaseCombination _baseCombination;

        private void OnEnable()
        {
            EventManager.OnCombinationRequested += StartCombination;
        }

        private void OnDisable()
        {
            EventManager.OnCombinationRequested -= StartCombination;
        }
        private void StartCombination(Piece pieceA, Piece pieceB)
        {
            OnCombinationStarted?.Invoke();
            if (!pieceA.TryGetComponent<Movable>(out var movable))
            {
                Debug.LogError("Movable component not found on pieceA!");
                return;
            }
            movable.StartMovingWithDuration(pieceB.transform.position,0.25f, onComplete: () => ActivateCombinationEffect(pieceA, pieceB));

        }
        private void ActivateCombinationEffect(Piece pieceA, Piece pieceB)
        {
            if (pieceA is not ICombinable combinableA || pieceB is not ICombinable combinableB)
                return;
            if (!_ruleSet.TryGetCombinationResult(pieceA.GetPieceType(), pieceB.GetPieceType(), out var combinationType))
            {
                Debug.LogWarning("Combination result not found");
                return;
            }
            _baseCombination = EventManager.OnCombinationSpawnRequested(combinationType, pieceB.Row,pieceB.Col);
            Cell spawnCell = pieceB.CurrentCell;
            pieceA.DestroyPieceInstantly();
            pieceB.DestroyPieceInstantly();
            _baseCombination.Init(spawnCell);
            _baseCombination.OnCombinationCompleted += CompleteCombination;
            _baseCombination.StartCombination(spawnCell.Row, spawnCell.Col);

        }

        private void CompleteCombination()
        {
            _baseCombination.OnCombinationCompleted -= CompleteCombination;
            OnCombinationCompleted?.Invoke();

        }
        
    }
}