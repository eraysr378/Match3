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
            EventManager.RequestPieceSpawn += SpawnPiece;
            EventManager.RequestFallingPieceSpawn += SpawnRandomNormalPiece;
        }

        private void OnDisable()
        {
            EventManager.RequestPieceSpawn -= SpawnPiece;
            EventManager.RequestFallingPieceSpawn -= SpawnRandomNormalPiece;
        }
  
        private Piece SpawnPiece(PieceType pieceType, int row,int col)
        {
            Piece piece = pieceFactory.GetPieceBasedOnType(pieceType);
            // Cell cell = GridManager.Instance.GetCellAt(row, col);
            // piece?.Init(cell.transform.position);
            // return piece;
            // BaseCell baseCell = GridManager.Instance.GetCellAt(0, 0);
            // Vector3 spawnPos = baseCell.transform.position + new Vector3(col,row,0);
            Vector3 spawnPos = GridManager.Instance.GridOrigin + new Vector3(col,row,0);
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
            BaseCell baseCell = GridManager.Instance.GetCellAt(row, col);
            Vector3 spawnPos = baseCell.transform.position + Vector3.up;
            return SpawnPieceAtPosition(randomPieceType, row, col, spawnPos);
        }
    }
}