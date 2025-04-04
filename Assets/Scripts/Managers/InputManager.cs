using Handlers;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputHandler userInputHandler;
        [SerializeField] private InputHandler autoInputHandler;
        private InputHandler _activeHandler;

        private void Start()
        {
            SetInputHandler(userInputHandler); 
        }

        private void SetInputHandler(InputHandler newHandler)
        {
            if (_activeHandler != null)
                _activeHandler.Disable();

            _activeHandler = newHandler;
            _activeHandler.Enable();
        }

        public void ToggleInputMode()
        {
            SetInputHandler(_activeHandler == userInputHandler ? autoInputHandler : userInputHandler);
        }
    }
}