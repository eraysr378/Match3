using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace ParticleEffects
{
    public class ParticleHandler : MonoBehaviour
    {
        [System.Serializable]
        public class EffectEntry
        {
            public ParticleType type;
            public ParticleSystem effect;
        }
        
        [SerializeField] private List<EffectEntry> effects = new List<EffectEntry>();

        private readonly Dictionary<ParticleType, ParticleSystem> _effectDict = new();

        private void Awake()
        {
            foreach (var entry in effects)
            {
                if (entry.effect != null && !_effectDict.ContainsKey(entry.type))
                    _effectDict.Add(entry.type, entry.effect);
            }
        }

        public void Play(ParticleType type)
        {
            if (_effectDict.TryGetValue(type, out var ps))
            {
                ParticleSystem particleSystem= Instantiate(ps,transform);
                particleSystem.transform.SetParent(null);
                // ps.gameObject.SetActive(true);
                // ps.Play();
            }
        }

        public void Stop(ParticleType type)
        {
            if (_effectDict.TryGetValue(type, out var ps))
            {
                // ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ps.gameObject.SetActive(false);

            }
        }

        public void StopAll()
        {
            foreach (var ps in _effectDict.Values)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }
}