using UnityEngine;

namespace Interfaces
{
    public interface IControlledActivatable
    {
        void WaitForActivation();
        void ForceActivate();
    }
}