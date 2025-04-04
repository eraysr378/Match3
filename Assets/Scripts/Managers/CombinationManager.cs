using Interfaces;
using Pieces;
using UnityEngine;

namespace Managers
{
    public class ActivatableCombinationManager : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.OnActivatableCombinationRequested += HandleCombination;
        }

        private void OnDisable()
        {
            EventManager.OnActivatableCombinationRequested -= HandleCombination;
        }
        private void HandleCombination(Piece pieceA, Piece pieceB)
        {
            if (!(pieceA is IActivatable activatableA) || !(pieceB is IActivatable activatableB)) return;

            // Decide which piece remains and which one merges
            Piece basePiece = pieceA;
            Piece mergingPiece = pieceB;

            if (pieceB is RainbowPiece) // Example: Keep RainbowPiece if available
            {
                basePiece = pieceB;
                mergingPiece = pieceA;
            }

            // Move merging piece onto base piece
            mergingPiece.transform.position = basePiece.transform.position;

            // Trigger the enhanced combination effect
            ExecuteSpecialCombination(basePiece, mergingPiece);

            // Remove the merging piece after combining
            mergingPiece.Explode();
        }

        private void ExecuteSpecialCombination(Piece basePiece, Piece mergingPiece)
        {
            if (basePiece is RainbowPiece && mergingPiece is RainbowPiece)
            {
                GridManager.Instance.Grid.DestroyAllPieces();
            }
            else if (basePiece is RainbowPiece)
            {
                GridManager.Instance.Grid.DestroyPiecesOfType(mergingPiece.PieceType);
            }
            else if (basePiece is BombPiece && mergingPiece is BombPiece)
            {
                GridManager.Instance.Grid.ExplodeLargeArea(basePiece, mergingPiece);
            }
            else if (basePiece is RocketPiece && mergingPiece is RocketPiece)
            {
                GridManager.Instance.Grid.ClearBoard();
            }
            else
            {
                GridManager.Instance.Grid.ExplodeMediumArea(basePiece, mergingPiece);
            }
        }
    }
}