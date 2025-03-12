using UnityEngine;
using Grid = GridRelated.Grid;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GridPropertiesSo", fileName = "GridProperties")]
    public class GridPropertiesSo : ScriptableObject
    {
        public Grid grid;
        public int width;
        public int height;
        public float gridSpriteOffset;
        public float elementSize;
        public Vector2 gridOffset;
        public Vector3 gridPlaygroundCenter;
        public CustomGridScriptableObject customGridSo;
    }
}