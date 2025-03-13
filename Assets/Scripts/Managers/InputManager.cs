using Cells;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private Camera _mainCamera;
        private Cell _lastTouchedCell; // Track last touched cell for "Enter" event

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Touchscreen.current == null || Touchscreen.current.primaryTouch.press.isPressed == false)
                return;
            Debug.Log("touchscreen pressed");
            TouchControl touch = Touchscreen.current.primaryTouch;
            Vector2 touchPosition = touch.position.ReadValue();


            Vector2 worldPoint = _mainCamera.ScreenToWorldPoint(touchPosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero); // No need for a direction in 2D

            if (hit.collider) // Ensure we hit something
            {
                Cell touchedCell = hit.collider.GetComponent<Cell>();
                if (touchedCell)
                {
                    HandleTouch(touch,touchedCell);
                }
            }
        }

        private void HandleTouch(TouchControl touch, Cell cell)
        {

            switch (touch.phase.ReadValue())
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    EventManager.OnPointerDownCell?.Invoke(cell);
                    _lastTouchedCell = cell;
                    break;

                case UnityEngine.InputSystem.TouchPhase.Moved:
                    if (_lastTouchedCell != cell) // Trigger "enter" only if different
                    {
                        EventManager.OnPointerEnterCell?.Invoke(cell);
                        _lastTouchedCell = cell;
                    }

                    break;

                case UnityEngine.InputSystem.TouchPhase.Ended:
                    EventManager.OnPointerUpCell?.Invoke(cell);
                    _lastTouchedCell = null;
                    break;
            }
        }
    }
}