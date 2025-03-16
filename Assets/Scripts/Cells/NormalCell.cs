using Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace Cells
{
 
    public class NormalCell : Cell, ISwappable,IMatchable
    {
        [SerializeField] private NormalCellSpritesSo spritesSo;
        
        public override void Init(int row, int col, Vector3 position, float elementSize,Transform parent)
        {
            base.Init(row, col, position, elementSize,parent);
            SetCellAppearance();
        }

        private void SetCellAppearance()
        {
            visual.sprite = spritesSo.GetSprite(cellType);
        }
        

        public void Swap(Cell other)
        {

        }


 
    }
}
