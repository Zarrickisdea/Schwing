using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl
{
    private CharacterModel model;
    private CharacterView view;
    private Vector2 lookVector;
    private Vector3 movementDirection = Vector3.zero;
    public CharacterControl(CharacterView characterView)
    {
        model = new CharacterModel();
        view = characterView;
        view.SetControl(this);
    }

    public void Move(InputAction moveAction)
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        if (moveVector.sqrMagnitude == 0)
        {
            return;
        }

        #region Camera Movement
        #endregion

        #region Character Movement
        movementDirection += moveVector.x * view.GetCameraRight() * model.WalkSpeed;
        movementDirection += moveVector.y * view.GetCameraForward() * model.WalkSpeed;
        
        //float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
        //float angle = Mathf.SmoothDampAngle(view.transform.eulerAngles.y, targetAngle, ref currentVelocity, 0.5f);
        //view.transform.rotation = Quaternion.Euler(0f, angle, 0f);

        view.Rb.AddForce(movementDirection, ForceMode.Impulse);
        movementDirection = Vector3.zero;

        Vector3 velocity = view.Rb.velocity;
        if (velocity.sqrMagnitude > model.MaxSpeed * model.MaxSpeed)
        {
            view.Rb.velocity = velocity.normalized * model.MaxSpeed + Vector3.up * view.Rb.velocity.y;
        }
        #endregion

    }

    public bool IsGrounded()
    {
        Ray ray = new Ray(view.transform.position + Vector3.up * 0.25f, Vector3.down);
        return Physics.Raycast(ray, out RaycastHit hit, 0.3f);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            view.Rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
    }

    public void Look(InputAction moveAction)
    {
        Vector3 lookDirection = view.Rb.velocity;
        lookDirection.y = 0;

        if (moveAction.ReadValue<Vector2>().sqrMagnitude > 0.1f && lookDirection.sqrMagnitude > 0.1f)
        {
            view.Rb.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
        else
        {
            view.Rb.angularVelocity = Vector3.zero;
        }
    }
}
