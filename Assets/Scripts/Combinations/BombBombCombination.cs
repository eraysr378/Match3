using System.Collections.Generic;
using Interfaces;
using Managers;
using UnityEngine;

namespace Pieces.CombinationPieces
{
    public class BombBombCombination : Combination
    {
        [SerializeField] private int explosionRadius;
        public override void ExecuteEffect(int row,int col)
        {
            List<IExplodable> explodables = GridManager.Instance.GetPiecesInRadius<IExplodable>(row, col, explosionRadius);

            foreach (var explodable in explodables)
            {
                explodable.Explode();
            }
            
            CompleteCombination();
            
        }

        protected override void CompleteCombination()
        {
            base.CompleteCombination();
            // DestroyPieceInstantly();
        }
    }
}