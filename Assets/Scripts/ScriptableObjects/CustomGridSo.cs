using GridRelated;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CustomGrid", fileName = "CustomGrid")]
    public class CustomGridSo : ScriptableObject
    {
        public CustomGrid customGrid;
    }
}