using System.Collections.Generic;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.PieceFactories
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
                if (!_pieceFactoriesByType.TryAdd(_pieceTypeKeys[i], _pieceFactoryValues[i]))
                {
                    Debug.LogError("Duplicate key");
                }
                
            }
        }

        public Piece CreatePieceBasedOnType(PieceType pieceType)
        {
            if (_pieceFactoriesByType.TryGetValue(pieceType, out var factory))
            {
                return factory.CreatePiece(pieceType);
            }
            return null;
        }
    }
}
