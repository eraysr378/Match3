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
            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                yield return null;
            }

            float duration = _animator.GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(duration);

            onComplete?.Invoke();
        }
    }
}