using System;
using System.Collections.Generic;
using Cells;
using Factories.GeneralFactories;
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
        [SerializeField] private GeneralPieceFactory pieceFactory;

        public void OnEnable()
        {
            EventManager.OnPieceSpawnRequested += SpawnPiece;
            EventManager.OnRandomNormalPieceSpawnRequested += SpawnRandomNormalPiece;
        }

        private void OnDisable()
        {
            EventManager.OnPieceSpawnRequested -= SpawnPiece;
            EventManager.OnRandomNormalPieceSpawnRequested -= SpawnRandomNormalPiece;
        }
  
        private Piece SpawnPiece(PieceType pieceType, int row, int col)
        {
            Piece piece = pieceFactory.GetPieceBasedOnType(pieceType);

            Vector2 pos = GridUtility.GridPositionToWorldPosition(row, col);
            Cell referenceCell = GridManager.Instance.GetCellAt(0, 0); // just to make sure piece is resized
            if (referenceCell != null)
            {
                piece?.transform.SetParent(referenceCell.transform);
            }
            piece?.Init(pos);
            return piece;
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