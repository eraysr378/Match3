using System;
using Cells;
using Interfaces;
using Misc;
using UnityEngine;

namespace Combinations
{
    public abstract class Combination : MonoBehaviour , IPoolableObject
    {
        [SerializeField] protected SpriteRenderer visual;
        public event Action OnCombinationCompleted;
        private CombinationType _combinationType;
        private CombinationAnimationHandler _animationHandler;

        public virtual void Awake()
        {
            _animationHandler = GetComponent<CombinationAnimationHandler>();
        }
        public void Init(Cell spawnCell)
        {
            transform.SetParent(spawnCell.transform);
            transform.localScale = Vector3.one;
        }

        protected abstract void ActivateCombination(int row,int col);

        protected virtual void CompleteCombination()
        {
            OnCombinationCompleted?.Invoke();
            _animationHandler.PlayEndAnimation(onComplete:DestroySelf);
        }

        public CombinationType GetCombinationType()
        {
            return _combinationType;
        }
        public void SetCombinationType(CombinationType type)
        {
             _combinationType = type;
        }
        protected virtual void DestroySelf()
        {
            Destroy(gameObject);
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
            // throw new NotImplementedException();
        }

        public virtual void OnReturnToPool()
        {
            // throw new NotImplementedException();
        }
    }
}