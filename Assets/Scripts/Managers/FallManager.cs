using System;
using System.Collections;
using System.Collections.Generic;
using Cells;
using Managers;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class FallManager : MonoBehaviour
    {
        public static event Action OnAllFallsCompleted;
        private Dictionary<int,Queue<BaseCell> > _queueDict = new Dictionary<int, Queue<BaseCell>>();
        private float minDelay = 0.02f;
        private float maxDelay = 0.1f;
        private float _activeFalls;
        private void OnEnable()
        {
            BaseCell.OnAnyRequestFall += HandlePieceFall;
            // FallHandler.OnAnyFallStarted += HandleAnyFallStarted;
            FallHandler.OnAnyFallCompleted += HandleAnyFallCompleted;
        }

        private void OnDisable()
        {
            BaseCell.OnAnyRequestFall -= HandlePieceFall;
            // FallHandler.OnAnyFallStarted -= HandleAnyFallStarted;
            FallHandler.OnAnyFallCompleted -= HandleAnyFallCompleted;
        }
        // private void HandleAnyFallStarted(FallHandler fallHandler)
        // {
        //     _activeFalls++;
        // }

        private void HandleAnyFallCompleted(FallHandler fallHandler)
        {
            _activeFalls--;
            if (_activeFalls == 0)
            {
                OnAllFallsCompleted?.Invoke();
            }
        }

        

        private void HandlePieceFall(BaseCell baseCell)
        {
            if (baseCell.CurrentPiece == null)
            {
                SpawnNewPiece(baseCell);
                // _queueDict.TryAdd(cell.Col, new Queue<Cell>());
                // _activeFalls++;
                // _queueDict[cell.Col].Enqueue(cell);
                // if (_queueDict[cell.Col].Count == 1)
                // {
                //     StartCoroutine(SpawnNewPieceDelayed( _queueDict[cell.Col]));
                // }
            }
        }

        private IEnumerator SpawnNewPieceDelayed(Queue<BaseCell> queue)
        {
            while (queue.Count != 0)
            {
                float delay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);
                SpawnNewPiece(queue.Dequeue());
            }
        }

        private void SpawnNewPiece(BaseCell baseCell)
        {
            Piece newPiece = EventManager.RequestFallingPieceSpawn(baseCell.Row, baseCell.Col);
            newPiece.SetCell(baseCell);
            newPiece.TryGetComponent<FallHandler>(out var fallHandler);
            fallHandler.FallTo(baseCell);
        }

        public bool IsBusy()
        {
            return _activeFalls > 0;
        }
    }
}