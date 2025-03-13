using Cells;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NormalCellSprites", menuName = "ScriptableObjects/NormalCellSprites")]
    public class NormalCellSpritesSo : ScriptableObject
    {
        public Sprite blueSprite;
        public Sprite redSprite;
        public Sprite greenSprite;

        public Sprite GetSprite(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.SquareNormalCell: return blueSprite;
                case CellType.CircleNormalCell: return redSprite;
                // case CellType.TriangleNormalCell: return greenSprite;
                default: return null;
            }
        }
    }
}