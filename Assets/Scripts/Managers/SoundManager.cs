using System.Collections.Generic;
using System.Linq;
using Database;
using Pools;
using SoundRelated;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField] private SoundDatabase soundDatabase;
        [SerializeField] private AudioSourcePool audioSourcePool;

        private Dictionary<SoundType, SoundEffectEntry> _soundMap;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializeMap();
                audioSourcePool.Initialize();
            }
        }

        private void InitializeMap()
        {
            _soundMap = soundDatabase.soundEffects
                .Where(e => e != null && e.clips.Length > 0)
                .ToDictionary(e => e.soundType, e => e);
        }

        public void PlaySound(SoundType type, float pitch = 1)
        {
           // if (_soundMap.TryGetValue(type, out var soundData))
            //{
            //    var clip = soundData.clips[Random.Range(0, soundData.clips.Length)];
            //    audioSourcePool.PlayOneShot(clip, pitch);
            //}
        }
    }
}