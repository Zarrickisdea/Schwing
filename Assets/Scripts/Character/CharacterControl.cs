using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl
{
    #region Character MVC
    private CharacterModel model;
    private CharacterView view;
    #endregion

    #region Private variables
    private Vector3 movementDirection = Vector3.zero;
    #endregion

    public CharacterControl(CharacterView characterView)
    {
        model = new CharacterModel();
        view = characterView;
        view.SetControl(this);
    }

    #region Movement
    public void Move(InputAction moveAction)
    {
        #region Character Movement
        movementDirection += moveAction.ReadValue<Vector2>().x * view.GetCameraRight() * model.WalkSpeed;
        movementDirection += moveAction.ReadValue<Vector2>().y * view.GetCameraForward() * model.WalkSpeed;

        view.Rb.AddForce(movementDirection, ForceMode.Impulse);
        movementDirection = Vector3.zero;

        Vector3 velocity = view.Rb.velocity;
        velocity.y = 0;
        if (velocity.sqrMagnitude > model.MaxSpeed * model.MaxSpeed)
        {
            view.Rb.velocity = velocity.normalized * model.MaxSpeed + Vector3.up * view.Rb.velocity.y;
        }
        #endregion

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
    #endregion

    #region Jumping
    public bool IsGrounded()
    {
        Ray ray = new Ray(view.transform.position + Vector3.up * 0.25f, Vector3.down);
        return Physics.Raycast(ray, out RaycastHit hit, 2f);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            movementDirection += Vector3.up * 10;
        }
    }
    #endregion

    #region Aiming
    public void CrosshairCheck()
    { 
        Ray ray = new Ray(view.PlayerCamera.transform.position, view.PlayerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Swing")))
        {
            view.Crosshair.color = Color.green;
        }
        else
        {
            view.Crosshair.color = Color.red;
        }
    }
    #endregion
}
