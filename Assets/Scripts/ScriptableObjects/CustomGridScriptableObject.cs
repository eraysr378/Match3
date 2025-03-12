using GridRelated;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CustomGrid", fileName = "CustomGrid")]
    public class CustomGridScriptableObject : ScriptableObject
    {
        public CustomGrid customGrid;
    }
}