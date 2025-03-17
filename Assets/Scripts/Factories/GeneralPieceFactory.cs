using System;
using System.Collections.Generic;
using Pieces;
using Misc;
using UnityEngine;

namespace Factories
{
    public class GeneralPieceFactory : MonoBehaviour
    {
        [SerializeField] private List<PieceType> _pieceTypeKeys;
        [SerializeField] private List<PieceFactory> _pieceFactoryValues;
        private Dictionary<PieceType, PieceFactory> _pieceFactoriesByType;
        
        private void Awake()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            _pieceFactoriesByType = new Dictionary<PieceType, PieceFactory>();
            for (int i = 0; i < Mathf.Min(_pieceTypeKeys.Count, _pieceFactoryValues.Count); i++)
            {
                _pieceFactoriesByType.Add(_pieceTypeKeys[i], _pieceFactoryValues[i]);
            }
        }

        public Piece CreateCellBasedOnType(PieceType pieceType)
        {
            if (_pieceFactoriesByType.TryGetValue(pieceType, out var factory))
            {
                return factory.CreateCell(pieceType);
            }
            return null;
        }

        // public Cell CreateRandomNormalCell()
        // {
        //     // Filter out only the CellTypes that are created by NormalCellFactory
        //     List<CellType> normalCellTypes = _cellFactoriesByType
        //         .Where(kvp => kvp.Value is NormalCellFactory) 
        //         .Select(kvp => kvp.Key) 
        //         .ToList();
        //
        //     if (normalCellTypes.Count == 0)
        //         throw new Exception("No NormalCellFactory found in the dictionary.");
        //
        //     CellType randomCellType = normalCellTypes[UnityEngine.Random.Range(0, normalCellTypes.Count)];
        //
        //     // Use the factory to create the cell
        //     return CreateCellBasedOnType(randomCellType);
        // }
  
    }
}
