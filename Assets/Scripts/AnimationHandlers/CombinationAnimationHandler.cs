using System;
using System.Collections;
using AnimationHandlers;
using UnityEngine;

namespace Combinations
{
    public class CombinationAnimationHandler : BaseAnimationHandler
    {
        private const string START_ANIMATION = "Start";
        private const string EFFECT_ANIMATION = "Effect";
        private const string END_ANIMATION = "End";
 
    
        public void PlayStartAnimation(Action onComplete)
        {
            PlayAnimation(START_ANIMATION, onComplete);
        }

        public void PlayEffectAnimation(Action onComplete)
        {
            PlayAnimation(EFFECT_ANIMATION, onComplete);
        }

        public void PlayEndAnimation(Action onComplete)
        {
           PlayAnimation(END_ANIMATION, onComplete);
        }
        

        
    }
}