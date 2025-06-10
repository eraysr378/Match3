using System;
using Cells;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combinations
{
    public abstract class BaseCombination : MonoBehaviour , IPoolableObject
    {
        public event Action OnCombinationCompleted;

        [SerializeField] protected SpriteRenderer visual;
        [SerializeField] private CombinationType combinationType;
        private CombinationAnimationHandler _animationHandler;

        public virtual void Awake()
        {
            _animationHandler = GetComponent<CombinationAnimationHandler>();
        }
        public void Init(BaseCell spawnBaseCell)
        {
            transform.SetParent(spawnBaseCell.transform);
            transform.localScale = Vector3.one;
        }

        protected abstract void ActivateCombination(int row,int col);

        protected virtual void CompleteCombination()
        {
            OnCombinationCompleted?.Invoke();
            _animationHandler.PlayEndAnimation(onComplete:OnReturnToPool);
        }

        public CombinationType GetCombinationType()
        {
            return combinationType;
        }
        
        public void StartCombination(int row, int col)
        {
            _animationHandler.PlayStartAnimation(() =>
            {
                _animationHandler.PlayEffectAnimation(null);
                ActivateCombination(row, col);
            });
        }


        public virtual void OnSpawn()
        {
            
        }

        public virtual void OnReturnToPool()
        {
            EventManager.OnCombinationReturnToPool?.Invoke(this);
        }
    }
}