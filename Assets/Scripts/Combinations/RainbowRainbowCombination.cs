using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Interfaces;
using Managers;
using Misc;
using Pieces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Combinations
{
    public class RainbowRainbowCombination : Combination
    {
        private PieceType _targetType;
        

        protected override void ExecuteEffect(int row, int col)
        {
            _targetType = PieceType.SquareNormalPiece;
            StartCoroutine(DestroyAllPieces());
        }

        private IEnumerator DestroyAllPieces()
        {
            Grid grid = GridManager.Instance.Grid;
            List<Piece> allPieces = grid.GetAllPieces().ToList();
            // allPieces = allPieces.OrderBy(_ => UnityEngine.Random.value).ToList();
            yield return new WaitForSeconds(1f);

            foreach (var piece in allPieces)
            {
                if (piece != null && piece.gameObject.activeSelf &&
                    piece.TryGetComponent<IExplodable>(out var explodable))
                    explodable.Explode();

            }
            yield return new WaitForSeconds(0.2f);

            CompleteCombination();
        }

    }
}