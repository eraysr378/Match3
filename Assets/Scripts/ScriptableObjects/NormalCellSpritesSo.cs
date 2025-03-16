using Misc;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NormalCellSprites", menuName = "ScriptableObjects/NormalCellSprites")]
    public class NormalCellSpritesSo : ScriptableObject
    {
        [System.Serializable]
        private struct CellSpritePair
        {
            public CellType cellType;
            public Sprite sprite;
        }

        [SerializeField] private CellSpritePair[] cellSprites;

        public Sprite GetSprite(CellType type)
        {
            foreach (var pair in cellSprites)
            {
                if (pair.cellType == type)
                {
                    return pair.sprite;
                }
            }

            Debug.LogError("Sprite not found for type: " + type);
            return null;
        }
    }
}