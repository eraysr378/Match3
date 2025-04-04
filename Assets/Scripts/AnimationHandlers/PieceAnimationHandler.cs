using System;
using System.Collections;
using AnimationHandlers;
using UnityEngine;

namespace Pieces
{
    [RequireComponent(typeof(Animator))]
    public class PieceAnimationHandler : BaseAnimationHandler
    {
        private const string ACTIVATE_ANIMATION = "Activate";
        private const string END_ANIMATION = "End";
        
        public void PlayActivateAnimation(Action onComplete)
        {
            PlayAnimation(ACTIVATE_ANIMATION, onComplete);

        }
        public void PlayEndAnimation(Action onComplete)
        {
            PlayAnimation(END_ANIMATION, onComplete);
        }

   
    }
}