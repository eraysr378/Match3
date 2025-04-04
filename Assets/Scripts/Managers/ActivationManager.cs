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
        [SerializeField] private int activeCount = 0;
        [SerializeField] private List<Piece>  activatedObjects = new List<Piece>(); 


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
            if (activeCount == 0)
            {
                OnActivationsStarted?.Invoke();
            }
            activeCount++;
            activatable.OnActivationCompleted += HandleActivationCompleted;
            Piece piece = activatable as Piece;
            activatedObjects.Add(piece);
        }

        private void HandleActivationCompleted(IActivatable activatable)
        {
            activatable.OnActivationCompleted -= HandleActivationCompleted;
            Piece piece = activatable as Piece;
            activatedObjects.Remove(piece);
            activeCount--;

            if (activeCount == 0)
            {
                OnActivationsCompleted?.Invoke();
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