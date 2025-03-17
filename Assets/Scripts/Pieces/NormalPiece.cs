using Cells;
using Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace Pieces
{
 
    public class NormalPiece : Piece, ISwappable,IMatchable
    {
        [SerializeField] private NormalPieceSpritesSo spritesSo;
        
        public override void Init( Vector3 position, float elementSize,Transform parent,Cell cell = null)
        {
            base.Init(position, elementSize,parent,cell);
            SetPieceAppearance();
        }

        private void SetPieceAppearance()
        {
            visual.sprite = spritesSo.GetSprite(pieceType);
        }
        

        public void Swap(Piece other)
        {

        }


 
    }
}
