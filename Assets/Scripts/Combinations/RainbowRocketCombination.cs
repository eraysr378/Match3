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
    public class RainbowRocketCombination : Combination
    {
        private PieceType _targetType;
        private List<RocketPiece> _spawnedRockets = new List<RocketPiece>();

        protected override void ExecuteEffect(int row,int col)
        {   
            _targetType = PieceType.SquareNormalPiece;
            StartCoroutine(SpawnRocketsCoroutine());
        }
        
        private IEnumerator SpawnRocketsCoroutine()
        {
            Grid grid = GridManager.Instance.Grid;
            List<Piece> matchingPieces = grid.GetPiecesByType(_targetType);
            matchingPieces = matchingPieces.OrderBy(_ => Random.value).ToList();

            foreach (var piece in matchingPieces)
            {
                if (piece != null)
                {
                    RocketPiece rocket = SpawnRocket(piece.Row, piece.Col);
                    _spawnedRockets.Add(rocket);
                    
                    Cell piecesCell = piece.CurrentCell;
                    piece.DestroyPieceInstantly();
                    rocket.SetCell(piecesCell);

                    yield return new WaitForSeconds(0.1f); 
                }
            }

            yield return new WaitForSeconds(0.1f); 

            ActivateAllRockets();
            CompleteCombination();
        }

        private RocketPiece SpawnRocket(int row, int col)
        {
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, row, col);

            if (piece is not RocketPiece rocketPiece)
                return null;

            if (Random.Range(0, 2) == 0) 
                rocketPiece.SetHorizontal();
            else 
                rocketPiece.SetVertical();

            return rocketPiece;
        }

        private void ActivateAllRockets()
        {
            _spawnedRockets = _spawnedRockets.OrderBy(_ => Random.value).ToList();

            foreach (var rocket in _spawnedRockets)
            {
                rocket.Activate();
            }
        }
    }
}
