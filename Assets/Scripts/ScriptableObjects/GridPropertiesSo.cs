using UnityEngine;
using UnityEngine.Serialization;
using Grid = GridRelated.Grid;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GridPropertiesSo", fileName = "GridProperties")]
    public class GridPropertiesSo : ScriptableObject
    {
        public Grid grid;
        public int width;
        public int height;
        public float cellSize;
        public float spacingRatio;
        public Vector2 gridOffset;
        public Vector3 gridPlaygroundCenter;
        public CustomGridScriptableObject customGridSo;
    }
}