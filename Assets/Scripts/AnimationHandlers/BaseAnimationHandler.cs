using System;
using System.Collections;
using UnityEngine;

namespace AnimationHandlers
{
    public abstract class BaseAnimationHandler : MonoBehaviour
    {
        private Animator _animator;
        private Action _onAnimationComplete;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        protected void PlayAnimation(string animationName,Action onComplete)
        {
            if (_animator.HasState(0, Animator.StringToHash(animationName)))
            {
                _animator.Play(animationName);
                StartCoroutine(WaitForAnimation(animationName, onComplete));
            }
            else
            {
                onComplete?.Invoke();
            }
        }

        private IEnumerator WaitForAnimation(string animationName, Action onComplete)
        {
            
            yield return new WaitUntil(() => 
                _animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));

            if (_animator.GetCurrentAnimatorStateInfo(0).loop && onComplete != null)
            {
                Debug.LogWarning("Are you Waiting Looped Animation?");
            }
            yield return new WaitUntil(() => 
                _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
                !_animator.IsInTransition(0));
        
            onComplete?.Invoke();
        }
    }
}