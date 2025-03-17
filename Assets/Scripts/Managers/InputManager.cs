using Pieces;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private Camera _mainCamera;
        private Piece _lastTouchedPiece; // Track last touched cell for "Enter" event

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
                Piece touchedPiece = hit.collider.GetComponent<Piece>();
                if (touchedPiece)
                {
                    HandleTouch(touch,touchedPiece);
                }
            }
        }

        private void HandleTouch(TouchControl touch, Piece piece)
        {

            switch (touch.phase.ReadValue())
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    EventManager.OnPointerDownCell?.Invoke(piece);
                    _lastTouchedPiece = piece;
                    break;

                case UnityEngine.InputSystem.TouchPhase.Moved:
                    if (_lastTouchedPiece != piece) // Trigger "enter" only if different
                    {
                        EventManager.OnPointerEnterCell?.Invoke(piece);
                        _lastTouchedPiece = piece;
                    }

                    break;

                case UnityEngine.InputSystem.TouchPhase.Ended:
                    EventManager.OnPointerUpCell?.Invoke(piece);
                    _lastTouchedPiece = null;
                    break;
            }
        }
    }
}