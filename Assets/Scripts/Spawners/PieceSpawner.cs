using System;
using System.Collections.Generic;
using UnityEngine;
using Pieces;
using Factories;
using Factories.PieceFactories;
using GridRelated;
using Managers;
using Misc;
using Random = System.Random;

public class PieceSpawner : MonoBehaviour
{
    private readonly Dictionary<PieceType, Queue<Piece>> _cellPools = new();
    [SerializeField] private GeneralPieceFactory pieceFactory;
    [SerializeField] private Transform cellParent;

    public void OnEnable()
    {
        EventManager.OnRequestPieceSpawn += SpawnPiece;
        EventManager.OnRequestRandomNormalPieceSpawn += SpawnRandomNormalPiece;
        EventManager.OnPieceReturnToPool += ReturnPieceToPool;
    }

    private void OnDisable()
    {
        EventManager.OnRequestPieceSpawn -= SpawnPiece;
        EventManager.OnRequestRandomNormalPieceSpawn -= SpawnRandomNormalPiece;
        EventManager.OnPieceReturnToPool -= ReturnPieceToPool;
    }

 
    private Piece SpawnPiece(PieceType pieceType, int row, int col)
    {
        Piece piece;
        if (_cellPools.TryGetValue(pieceType, out Queue<Piece> pool) && pool.Count > 0)
        {
            // Debug.Log("pool");
            piece = pool.Dequeue();
            piece.gameObject.SetActive(true);
            piece.OnSpawn();
        }
        else
        {
            // Debug.Log("factory");

            piece = pieceFactory.CreatePieceBasedOnType(pieceType);
        }
        Vector2 pos = GridUtility.GridPositionToWorldPosition(row, col);
        piece?.Init( pos, GridUtility.PropertiesSo.elementSize, cellParent);
        return piece;
    }

    private void ReturnPieceToPool(Piece piece)
    {
        if (!_cellPools.ContainsKey(piece.GetPieceType()))
        {
            _cellPools[piece.GetPieceType()] = new Queue<Piece>();
        }

        _cellPools[piece.GetPieceType()].Enqueue(piece);
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