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

        public (Sprite, Color) GetAppearance(PieceType pieceType)
        {
            foreach (var pair in pieceSprites)
            {
                if (pair.pieceType == pieceType)
                {
                    return (pair.sprite, pair.color);
                }
            }

            Debug.LogError("Sprite not found for type: " + pieceType);
            return (null, default);
        }
    }
}