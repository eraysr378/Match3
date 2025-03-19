using Cells;
using Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace Pieces
{
    public class NormalPiece : Piece, ISwappable, IMatchable
    {
        [SerializeField] private NormalPieceSpritesSo spritesSo;

        public override void Init(Vector3 position)
        {
            base.Init(position);
            SetPieceAppearance();
        }

        public override void Init(Cell cell)
        {
            base.Init(cell);
            SetPieceAppearance();
        }

        private void SetPieceAppearance()
        {
            visual.sprite = spritesSo.GetSprite(pieceType);
            visual.color = spritesSo.GetColor(pieceType);
        }


        public void Swap(Piece other)
        {
        }
    }
}