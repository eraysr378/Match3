using System;
using Cells;
using Combinations;
using CombinationSystem;
using Interfaces;
using Misc;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;
using Utils;

namespace Managers
{
    public class CombinationManager : MonoBehaviour
    {
        public static event Action OnCombinationStarted;
        public static event Action OnCombinationCompleted;
        private readonly CombinationRuleSet _ruleSet = new CombinationRuleSet();
        private BaseCombination _baseCombination;
        private CellDirtyTracker _cellDirtyTracker;

        private void Start()
        {
            _cellDirtyTracker = new CellDirtyTracker();
        }

        private void OnEnable()
        {
            EventManager.RequestCombination += StartCombination;
        }

        private void OnDisable()
        {
            EventManager.RequestCombination -= StartCombination;
        }
        private void StartCombination(Piece pieceA, Piece pieceB)
        {
            OnCombinationStarted?.Invoke();
            // Debug.LogWarning("Combination Started");
            if (!pieceA.TryGetComponent<Movable>(out var movable))
            {
                Debug.LogError("Movable component not found on pieceA!");
                return;
            }
            movable.StartMovingWithDuration(pieceB.transform.position,0.25f, onComplete: () => ActivateCombinationEffect(pieceA, pieceB));

        }
        private void ActivateCombinationEffect(Piece pieceA, Piece pieceB)
        {
            if (!_ruleSet.TryGetCombinationResult(pieceA.GetPieceType(), pieceB.GetPieceType(), out var combinationType))
            {
                Debug.LogWarning("Combination result not found");
                return;
            }
            _baseCombination = EventManager.RequestCombinationSpawn(combinationType, pieceB.Row,pieceB.Col);
            BaseCell spawnBaseCell = pieceB.CurrentCell;
            _cellDirtyTracker.Mark(pieceA.CurrentCell);
            _cellDirtyTracker.Mark(pieceB.CurrentCell);
            pieceA.DestroyPieceInstantly();
            pieceB.DestroyPieceInstantly();
            _baseCombination.Init(spawnBaseCell);
            _baseCombination.OnCombinationCompleted += CompleteCombination;
            _baseCombination.StartCombination(spawnBaseCell.Row, spawnBaseCell.Col);

        }

        private void CompleteCombination()
        {
            _baseCombination.OnCombinationCompleted -= CompleteCombination;
            _cellDirtyTracker.ClearAll();
            OnCombinationCompleted?.Invoke();
            // Debug.LogWarning("Combination Completed");
        }
    }
}