using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CharacterView : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Image crosshair;
    
    private Schwing playerActions;
    private InputAction moveAction;

    public Rigidbody Rb { get => rb; }
    public Image Crosshair { get => crosshair; }
    public Camera PlayerCamera { get => playerCamera; }

    public SpringJoint swingJoint;
    public LineRenderer lineRenderer;

    private CharacterControl control;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // center the cursor to the crosshair
        Cursor.visible = false;
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        Cursor.SetCursor(null, center, CursorMode.ForceSoftware);
        control = new CharacterControl(this);
        playerActions = new Schwing();
    }

    private void OnEnable()
    {
        playerActions.Player.Jump.started += control.Jump;
        playerActions.Player.Fire.started += control.StartSwing;
        playerActions.Player.Fire.canceled += control.EndSwing;
        moveAction = playerActions.Player.Move;
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Jump.started -= control.Jump;
        playerActions.Player.Fire.started -= control.StartSwing;
        playerActions.Player.Fire.canceled -= control.EndSwing;
        playerActions.Player.Disable();
    }

    private void Update()
    {
        control.CrosshairCheck();
    }

    private void FixedUpdate()
    {
        control.Look(moveAction);
        control.Move(moveAction);
    }

    private void LateUpdate()
    {
        DrawSwingLine();
    }

    private void DrawSwingLine()
    {
        if (swingJoint == null)
        {
            return;
        }

        lineRenderer.SetPosition(0, swingJoint.connectedAnchor);
        lineRenderer.SetPosition(1, transform.position);
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

    #if UNITY_EDITOR
    public float spring;
    public float damper;
    public float massScale;
    #endif
}
