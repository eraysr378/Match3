using System;
using System.Collections.Generic;
using Pieces.Behaviors;
using UnityEngine;

namespace Managers
{
    public class FillManager : MonoBehaviour
    {
        
        public static event Action OnAllFillsCompleted;
        public int _activeFills = 0;
        public List<FillHandler> fillables = new List<FillHandler>();

        private void OnEnable()
        {
            FillHandler.OnAnyFillStarted += HandleAnyFillStarted;
            FillHandler.OnAnyFillCompleted += HandleAnyFillCompleted;
        }
        
        private void OnDisable()
        {
            FillHandler.OnAnyFillStarted -= HandleAnyFillStarted;
            FillHandler.OnAnyFillCompleted -= HandleAnyFillCompleted;
        }

        private void HandleAnyFillStarted(FillHandler fillHandler)
        {
            fillables.Add(fillHandler);
            _activeFills++;
        }

        private void HandleAnyFillCompleted(FillHandler fillHandler)
        {
            fillables.Remove(fillHandler);
            _activeFills--;
            if (_activeFills == 0)
            {
                OnAllFillsCompleted?.Invoke();
            }
        }

        public bool IsBusy()
        {
            return _activeFills > 0;
        }
        
    }
}