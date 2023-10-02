using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CharacterView : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera playerCamera;
    
    private Schwing playerActions;
    private InputAction moveAction;

    public Rigidbody Rb { get => rb; }

    private CharacterControl control;

    private void Awake()
    {
        control = new CharacterControl(this);
        playerActions = new Schwing();
    }

    private void OnEnable()
    {
        playerActions.Player.Jump.started += control.Jump;
        moveAction = playerActions.Player.Move;
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Jump.started -= control.Jump;
        playerActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        control.Look(moveAction);
        control.Move(moveAction);
    }

    public void SetControl(CharacterControl control)
    {
        this.control = control;
    }

    public Vector3 GetCameraForward()
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    public Vector3 GetCameraRight()
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }
}
