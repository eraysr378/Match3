using System.Collections.Generic;
using Misc;
using UnityEngine;


namespace MatchSystem
{
    [CreateAssetMenu(menuName = "Match/Pattern")]
    public class MatchPattern : ScriptableObject
    {
        public PieceType resultPieceType;
        public Vector2Int spawnOffset; // Relative to match origin
        public List<Vector2Int> offsets; // Relative positions for matching pieces
    }
}