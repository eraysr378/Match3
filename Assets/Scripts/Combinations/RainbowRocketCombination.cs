using System.Collections;
using System.Collections.Generic;
using Cells;
using Managers;
using Misc;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Pieces.CombinationPieces
{
    public class RainbowRocketCombination : Combination
    {
        private PieceType _targetType;
        private List<RocketPiece> _spawnedRockets = new List<RocketPiece>();

        public override void ExecuteEffect(int row,int col)
        {   
            _targetType = PieceType.SquareNormalPiece;
            StartCoroutine(SpawnRocketsCoroutine());
        }
        
        protected override void CompleteCombination()
        {
            base.CompleteCombination();
            // DestroyPieceInstantly();
        }
        
        private IEnumerator SpawnRocketsCoroutine()
        {
            Grid grid = GridManager.Instance.Grid;
            List<Piece> matchingPieces = grid.GetPiecesByType(_targetType);
            
            foreach (var piece in matchingPieces)
            {
                if (piece != null)
                {
                    RocketPiece rocket = SpawnRocket(piece.Row, piece.Col);
                    _spawnedRockets.Add(rocket);
                    
                    piece.DestroyPieceInstantly();

                    yield return new WaitForSeconds(0.2f); 
                }
            }

            yield return new WaitForSeconds(0.5f); 

            ActivateAllRockets();
            CompleteCombination();
        }

        private RocketPiece SpawnRocket(int row, int col)
        {
            Grid grid = GridManager.Instance.Grid;
            Cell referenceCell = grid.GetCell(0, 0); // Used for sizing the rocket

            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, row, col);
            piece.transform.SetParent(referenceCell.transform); 
            piece.transform.localScale = Vector3.one;

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
            foreach (var rocket in _spawnedRockets)
            {
                rocket.Activate();
            }
        }
    }
}
