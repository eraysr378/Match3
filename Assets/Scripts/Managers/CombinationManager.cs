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
        public static event Action OnCombiantionStarted;
        public static event Action OnCombinationCompleted;
        private readonly CombinationRuleSet _ruleSet = new CombinationRuleSet();
        private Combination _combination;

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
            OnCombiantionStarted?.Invoke();
            if (!pieceA.TryGetComponent<Movable>(out var movable))
            {
                Debug.LogError("Movable component not found on pieceA!");
                return;
            }
            movable.StartMoving(pieceB.transform.position,0.25f, onComplete: () => ActivateCombinationEffect(pieceA, pieceB));

        }
        private void ActivateCombinationEffect(Piece pieceA, Piece pieceB)
        {
            if (!(pieceA is ICombinable combinableA) || !(pieceB is ICombinable combinableB))
                return;
            if (!_ruleSet.TryGetCombinationResult(pieceA.GetPieceType(), pieceB.GetPieceType(), out var combinationType))
            {
                Debug.LogWarning("Combination result not found");
                return;
            }
            _combination = EventManager.OnCombinationSpawnRequested(combinationType, pieceB.Row,pieceB.Col);
            Cell spawnCell = pieceB.CurrentCell;
            pieceA.DestroyPieceInstantly();
            pieceB.DestroyPieceInstantly();
            _combination.Init(spawnCell);
            _combination.OnCombinationCompleted += CompleteCombination;
            _combination.StartCombination(spawnCell.Row, spawnCell.Col);

        }

        private void CompleteCombination()
        {
            _combination.OnCombinationCompleted -= CompleteCombination;
            OnCombinationCompleted?.Invoke();

        }
        
    }
}