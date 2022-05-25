using UnityEngine;

namespace EventSystem
{
    public class InputHandler : MonoBehaviour
    {
        // Previous input values for input delta calculations
        private float previousHorizontal;
        private float previousVertical;
        private Vector3 previousMouse = Vector3.zero;
        
        private void Update()
        {
            // Movement axis changes
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 movementDelta = new Vector2(horizontal - previousHorizontal, vertical - previousVertical);
            if (movementDelta != Vector2.zero)
            {
                GameEvents.current.OnMovementInputChanged(new Vector2(horizontal, vertical), movementDelta);
            }
            
            // Check mouse movement
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseDelta = mousePos - previousMouse;
            if (mouseDelta != Vector3.zero)
            {
                GameEvents.current.OnMouseMoved(mousePos, mouseDelta);
            }

            // Update previous values
            previousHorizontal = horizontal;
            previousVertical = vertical;
            previousMouse = mousePos;
        }
    }
}
