using UnityEditor.SceneManagement;
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
    private bool isSwinging;
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
        movementDirection += moveAction.ReadValue<Vector2>().x * view.GetCameraRight();
        movementDirection += moveAction.ReadValue<Vector2>().y * view.GetCameraForward();

        if (isSwinging)
        {
            movementDirection *= model.SwingSpeed;
        }
        else
        {
            movementDirection *= model.WalkSpeed;
        }

        view.Rb.AddForce(movementDirection, ForceMode.Impulse);
        movementDirection = Vector3.zero;

        float currentMaxSpeed = isSwinging ? model.MaxSwingSpeed : model.MaxSpeed;

        Vector3 velocity = view.Rb.velocity;
        velocity.y = 0;
        if (velocity.sqrMagnitude > currentMaxSpeed * currentMaxSpeed)
        {
            view.Rb.velocity = velocity.normalized * currentMaxSpeed + Vector3.up * view.Rb.velocity.y;
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
            movementDirection += Vector3.up * model.JumpForce;
        }
    }
    #endregion

    #region Crosshair
    public RaycastHit CrosshairCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(view.Crosshair.transform.position);

        RaycastHit hit = new RaycastHit();
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, model.SwingLength, LayerMasks.Swing))
        {
            hit = hitInfo;
            view.Crosshair.color = Color.green;
        }
        else if (Physics.SphereCast(ray, 0.5f, out RaycastHit sphereHitInfo, model.SwingLength, LayerMasks.Swing))
        {
            hit = sphereHitInfo;
            view.Crosshair.color = Color.green;
        }
        else
        {
            view.Crosshair.color = Color.red;
        }
        return hit;
    }
    #endregion

    #region Swinging
    public void StartSwing(InputAction.CallbackContext context)
    {
        RaycastHit hit = CrosshairCheck();

        if (hit.collider == null)
        {
            return;
        }
        view.swingJoint = view.gameObject.AddComponent<SpringJoint>();
        view.swingJoint.autoConfigureConnectedAnchor = false;
        view.swingJoint.connectedAnchor = hit.point;

        float distance = Vector3.Distance(view.transform.position, hit.point);

        view.swingJoint.maxDistance = distance * 0.8f;
        view.swingJoint.minDistance = distance * 0.25f;

        view.swingJoint.spring = view.spring;
        view.swingJoint.damper = view.damper;
        view.swingJoint.massScale = view.massScale;

        view.lineRenderer.positionCount = 2;
        isSwinging = true;
    }

    public void EndSwing(InputAction.CallbackContext context)
    {
        if (view.swingJoint == null)
        {
            return;
        }
        Object.Destroy(view.swingJoint);
        view.lineRenderer.positionCount = 0;
        isSwinging = false;
    }
    #endregion
}
