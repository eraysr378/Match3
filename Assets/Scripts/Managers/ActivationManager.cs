using System;
using System.Collections.Generic;
using UnityEngine;
using Pieces;
using Interfaces;

namespace Managers
{
    public class ActivationManager : MonoBehaviour
    {
        public static event Action OnActivationsStarted;
        public static event Action OnActivationsCompleted;
        [SerializeField] private int _activeCount = 0;
        private List<IActivatable> _activePieces = new List<IActivatable>();


        private void OnEnable()
        {
            EventManager.OnPieceActivated += OnPieceActivated;
        }

        private void OnDisable()
        {
            EventManager.OnPieceActivated -= OnPieceActivated;
        }


        private void OnPieceActivated(IActivatable activatable)
        {
            if (_activeCount == 0)
            {
                OnActivationsStarted?.Invoke();
                // Debug.Log("Activation started");    
            }
            _activePieces.Add(activatable);
            _activeCount++;
            activatable.OnActivationCompleted += HandleActivationCompleted;
        }

        private void HandleActivationCompleted(IActivatable activatable)
        {
            activatable.OnActivationCompleted -= HandleActivationCompleted;

            _activeCount--;

            if (_activeCount == 0) // All activations completed
            {
                _activePieces.Clear();
                OnActivationsCompleted?.Invoke();
                // Debug.Log("Activation end");    

            }
        }

        public bool TryActivatePiece(Piece piece)
        {
            if (piece is IActivatable activatable)
            {
                activatable.Activate();
                return true;
            }

            return false;
        }
    }
}