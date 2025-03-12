using Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace Cells
{
    public enum NormalCellType
    {
        Blue,
        Red,
        Green
    }
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
            int saveRow = Row;
            int saveCol = Col;
            Vector3 savePosition = transform.position;
            SetRow(other.Row);
            SetCol(other.Col);
            transform.position = other.transform.position;
            other.SetRow(saveRow);
            other.SetCol(saveCol);
            other.transform.position = savePosition;
        }


    }
}
