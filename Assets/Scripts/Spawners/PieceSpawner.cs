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
            EventManager.OnFallingPieceSpawnRequested += SpawnRandomNormalPiece;
        }

        private void OnDisable()
        {
            EventManager.OnPieceSpawnRequested -= SpawnPiece;
            EventManager.OnFallingPieceSpawnRequested -= SpawnRandomNormalPiece;
        }
  
        private Piece SpawnPiece(PieceType pieceType, int row,int col)
        {
            Piece piece = pieceFactory.GetPieceBasedOnType(pieceType);
            // Cell cell = GridManager.Instance.GetCellAt(row, col);
            // piece?.Init(cell.transform.position);
            // return piece;
            Cell cell = GridManager.Instance.GetCellAt(0, 0);
            Vector3 spawnPos = cell.transform.position + new Vector3(col,row,0);
            piece?.Init(spawnPos);
            return piece;
        }

        private Piece SpawnPieceAtPosition(PieceType pieceType,int row,int col ,Vector3 pos)
        {
            Piece piece = pieceFactory.GetPieceBasedOnType(pieceType);
            piece?.Init(new Vector3(pos.x, pos.y, pos.z));
            return piece;
        }

        private Piece SpawnRandomNormalPiece(int row, int col)
        {
            Array values = Enum.GetValues(typeof(NormalPieceType));
            var random = new Random();
            NormalPieceType randomNormalPiece = (NormalPieceType)values.GetValue(random.Next(values.Length));
            PieceType randomPieceType = (PieceType)randomNormalPiece;
            Cell cell = GridManager.Instance.GetCellAt(row, col);
            Vector3 spawnPos = cell.transform.position + Vector3.up;
            return SpawnPieceAtPosition(randomPieceType, row, col, spawnPos);
        }
    }
}