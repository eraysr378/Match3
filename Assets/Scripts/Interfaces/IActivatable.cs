using System;

namespace Interfaces
{
    public interface IActivatable
    {
        public event Action<IActivatable> OnActivationCompleted;
        public void Activate();
        public bool IsActivated { get; }
    }
}