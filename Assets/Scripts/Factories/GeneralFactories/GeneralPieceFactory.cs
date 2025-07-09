using System;
using System.Collections.Generic;
using Factories.BaseFactories;
using Factories.PieceFactories;
using Managers;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.GeneralFactories
{
    public class GeneralPieceFactory : MonoBehaviour
    {
        [SerializeField] private List<BasePieceFactory> pieceFactoryList;
        [SerializeField] private Transform pieceParent;

        private void OnEnable()
        {
            EventManager.ReturnPieceToPool += ReturnToPool;
        }

        private void OnDisable()
        {
            EventManager.ReturnPieceToPool -= ReturnToPool;
            foreach (var factory in pieceFactoryList)
            {
                factory.ResetPool();
            }
        }

        private void Awake()
        {
            foreach (var factory in pieceFactoryList)
            {
                factory.Initialize(pieceParent);
            }
        }

        public Piece GetPieceBasedOnType(PieceType pieceType)
        {
            foreach (var factory in pieceFactoryList)
            {
                if (factory.CanCreatePiece(pieceType))
                {
                    return factory.Get();
                }
            }
            return null;
        }

        private void ReturnToPool(Piece piece)
        {
            foreach (var factory in pieceFactoryList)
            {
                if (factory.CanCreatePiece(piece.GetPieceType()))
                {
                    factory.ReturnToPool(piece);
                    return;
                }
            }
        }
    }
}
