using System;
using Pieces;

namespace Interfaces
{
    public interface IActivatable
    {
        public event Action<IActivatable> OnActivationCompleted;
        public void Activate();
    }
}