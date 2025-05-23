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

        private void OnEnable()
        {
            EventManager.OnPieceReturnToPool += ReturnToPool;
        }

        private void OnDisable()
        {
            EventManager.OnPieceReturnToPool -= ReturnToPool;
            foreach (var factory in pieceFactoryList)
            {
                factory.ResetPool();
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
