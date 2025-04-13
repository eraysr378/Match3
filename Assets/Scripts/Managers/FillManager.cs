using System;
using System.Collections.Generic;
using Pieces.Behaviors;
using UnityEngine;

namespace Managers
{
    public class FillManager : MonoBehaviour
    {
        public static event Action OnFillStarted;
        public static event Action OnAllFillsCompleted;
        public int _activeFills = 0;
        public List<Fillable> fillables = new List<Fillable>();

        private void OnEnable()
        {
            Fillable.OnFillStarted += HandleFillStarted;
            Fillable.OnFillCompleted += HandleFillCompleted;
        }

        private void OnDisable()
        {
            Fillable.OnFillStarted -= HandleFillStarted;
            Fillable.OnFillCompleted -= HandleFillCompleted;
        }

        private void HandleFillStarted(Fillable fillable)
        {
            fillables.Add(fillable);
            if (_activeFills == 0)
            {
                OnFillStarted?.Invoke();
            }
            _activeFills++;
        }

        private void HandleFillCompleted(Fillable fillable)
        {
            fillables.Remove(fillable);
            _activeFills--;
            if (_activeFills == 0)
            {
                OnAllFillsCompleted?.Invoke();
            }
        }
        
    }
}