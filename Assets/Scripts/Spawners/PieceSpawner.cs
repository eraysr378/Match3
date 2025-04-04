using System;
using System.Collections.Generic;
using Cells;
using Factories.PieceFactories;
using GridRelated;
using Managers;
using Misc;
using Pieces;
using UnityEngine;
using Random = System.Random;

namespace Spawners
{
    public class PieceSpawner : MonoBehaviour
    {
        private readonly Dictionary<PieceType, Queue<Piece>> _piecePools = new();
        [SerializeField] private GeneralPieceFactory pieceFactory;

        public void OnEnable()
        {
            EventManager.OnPieceSpawnRequested += SpawnPiece;
            EventManager.OnRandomNormalPieceSpawnRequested += SpawnRandomNormalPiece;
            EventManager.OnPieceReturnToPool += ReturnPieceToPool;
        }

        private void OnDisable()
        {
            EventManager.OnPieceSpawnRequested -= SpawnPiece;
            EventManager.OnRandomNormalPieceSpawnRequested -= SpawnRandomNormalPiece;
            EventManager.OnPieceReturnToPool -= ReturnPieceToPool;
        }
  
        private Piece SpawnPiece(PieceType pieceType, int row, int col)
        {
            Piece piece;
            if (_piecePools.TryGetValue(pieceType, out Queue<Piece> pool) && pool.Count > 0)
            {
                piece = pool.Dequeue();
                piece.gameObject.SetActive(true);
                piece.OnSpawn();
            }
            else
            {
                piece = pieceFactory.CreatePieceBasedOnType(pieceType);
            }

            Vector2 pos = GridUtility.GridPositionToWorldPosition(row, col);
            Cell referenceCell = GridManager.Instance.Grid?.GetCell(0, 0); // just to make sure piece is resized
            if (referenceCell != null)
            {
                piece?.transform.SetParent(referenceCell.transform);
            }
            piece?.Init(pos);
            return piece;
        }

        private void ReturnPieceToPool(Piece piece)
        {
            if (!_piecePools.ContainsKey(piece.GetPieceType()))
            {
                _piecePools[piece.GetPieceType()] = new Queue<Piece>();
            }

            _piecePools[piece.GetPieceType()].Enqueue(piece);
        }

        private Piece SpawnRandomNormalPiece(int row, int col)
        {
            Array values = Enum.GetValues(typeof(NormalPieceType));
            var random = new Random();
            NormalPieceType randomNormalPiece = (NormalPieceType)values.GetValue(random.Next(values.Length));
            PieceType randomPieceType = (PieceType)randomNormalPiece;

            return SpawnPiece(randomPieceType, row, col);
        }
    }
}