using Misc;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NormalPieceSprites", menuName = "ScriptableObjects/NormalPieceSprites")]
    public class NormalPieceSpritesSo : ScriptableObject
    {
        [System.Serializable]
        private struct PieceSpritePair
        {
            public PieceType pieceType;
            public Sprite sprite;
            public Color color;
        }

        [SerializeField] private PieceSpritePair[] pieceSprites;

        public Sprite GetSprite(PieceType type)
        {
            foreach (var pair in pieceSprites)
            {
                if (pair.pieceType == type)
                {
                    return pair.sprite;
                }
            }

            Debug.LogError("Sprite not found for type: " + type);
            return null;
        }
        public Color GetColor(PieceType type)
        {
            foreach (var pair in pieceSprites)
            {
                if (pair.pieceType == type)
                {
                    return pair.color;
                }
            }

            Debug.LogError("Sprite not found for type: " + type);
            return Color.white;
        }
    }
}