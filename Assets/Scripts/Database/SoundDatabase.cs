using System.Collections.Generic;
using SoundRelated;
using UnityEngine;

namespace Database
{
    [CreateAssetMenu(menuName = "Audio/Sound Database")]
    public class SoundDatabase : ScriptableObject
    {
        public List<SoundEffectEntry> soundEffects;
    }
}