using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCharacter : MonoBehaviour
{
    
    [SerializeField] private CharacterController controller;
    private Vector2 inputVector;
    public void InputHandler(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            inputVector = context.ReadValue<Vector2>();
        }
    }

    private void Update()
    {
        Vector3 move = new Vector3(inputVector.x, 0, inputVector.y);
        controller.Move(move * Time.deltaTime * 10);
    }
}
