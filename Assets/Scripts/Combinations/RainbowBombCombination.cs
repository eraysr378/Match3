using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Managers;
using Misc;
using Pieces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Combinations
{
    public class RainbowBombCombination : Combination
    {
        private PieceType _targetType;
        private List<BombPiece> _spawnedBombs = new List<BombPiece>();

        protected override void ExecuteEffect(int row, int col)
        {
            _targetType = PieceType.SquareNormalPiece;
            StartCoroutine(SpawnBombsCoroutine());
        }

        private IEnumerator SpawnBombsCoroutine()
        {
            Grid grid = GridManager.Instance.Grid;
            List<Piece> matchingPieces = grid.GetPiecesByType(_targetType);
            matchingPieces = matchingPieces.OrderBy(_ => Random.value).ToList();

            foreach (var piece in matchingPieces)
            {
                if (piece != null)
                {
                    BombPiece bomb = SpawnBomb(piece.Row, piece.Col);
                    _spawnedBombs.Add(bomb);
                    Cell piecesCell = piece.CurrentCell;
                    piece.DestroyPieceInstantly();
                    bomb.SetCell(piecesCell);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return new WaitForSeconds(0.1f);

            ActivateAllBombs();
            CompleteCombination();
        }

        private BombPiece SpawnBomb(int row, int col)
        {
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.BombPiece, row, col);

            if (piece is not BombPiece bombPiece)
                return null;


            return bombPiece;
        }

        private void ActivateAllBombs()
        {
            _spawnedBombs = _spawnedBombs.OrderBy(_ => Random.value).ToList();

            foreach (var bomb in _spawnedBombs)
            {
                bomb.Activate();
            }
        }
    }
}