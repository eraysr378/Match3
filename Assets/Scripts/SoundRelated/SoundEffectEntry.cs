using UnityEngine;

namespace SoundRelated
{
    [System.Serializable]
    public class SoundEffectEntry
    {
        public SoundType soundType;
        public AudioClip[] clips;
    }
    public enum SoundType
    {
        Swap,
        Match,
        InvalidSwap,
        SpecialMatch,
        Explosion,
        GameWin,
        GameLose,
        ButtonClick,
        BombExplosion,
        RainbowLaser,
        RocketLaunch
    }
}