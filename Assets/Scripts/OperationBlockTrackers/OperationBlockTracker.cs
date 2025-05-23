using System;
using UnityEngine;

namespace OperationBlockTrackers
{
    public abstract class OperationBlockTracker : MonoBehaviour
    {
        public event Action OnAllOperationsCompleted;
        [SerializeField]private int _activeOperations = 0;
        
        protected virtual void OnEnable()
        {
            SubscribeEvents();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeEvents();
        }

        protected abstract void SubscribeEvents();
        protected abstract void UnsubscribeEvents();

        protected void IncreaseActiveOperations()
        {
            _activeOperations++;
        }

        protected void DecreaseActiveOperations()
        {
            _activeOperations--;
            if (_activeOperations > 0) return;
            
            _activeOperations = 0;
            OnAllOperationsCompleted?.Invoke();
        }

        public bool HasActiveOperations() => _activeOperations > 0;
    }
}