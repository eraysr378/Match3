using Interfaces;
using UnityEngine;

namespace Pieces
{
    public class BombPiece: Piece, IActivatable,ISwappable
    {
        public bool IsActivated { get; private set; }

        public void Activate()
        {
            if (IsActivated)
            {
                Debug.Log("Already activated");
                return;
            }
            IsActivated = true;
            Debug.Log("Activated");
        }

        public void Swap(Piece other)
        {
            throw new System.NotImplementedException();
        }
    }
}