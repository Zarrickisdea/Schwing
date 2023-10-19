using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CharacterView : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Image crosshair;
    [SerializeField] private LevelGenerator levelGenerator;

    #endregion

    #region Private Variables

    private Schwing playerActions;
    private InputAction moveAction;
    private Subject levelSubject = new Subject();

    #endregion

    #region Properties

    public Rigidbody Rb { get => rb; }
    public Image Crosshair { get => crosshair; }
    public Camera PlayerCamera { get => playerCamera; }

    #endregion

    #region Public Variables

    public SpringJoint swingJoint;
    public LineRenderer lineRenderer;

    public float spring;
    public float damper;
    public float massScale;

    #endregion

    #region Character control

    private CharacterControl control;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        Cursor.SetCursor(null, center, CursorMode.ForceSoftware);

        control = new CharacterControl(this);
        playerActions = new Schwing();

        if (levelGenerator != null)
        {
            levelSubject.AddObserver(levelGenerator);
        }
    }

    private void OnEnable()
    {
        playerActions.Player.Jump.started += control.Jump;
        playerActions.Player.Fire.started += control.StartSwing;
        playerActions.Player.Fire.canceled += control.EndSwing;
        moveAction = playerActions.Player.Move;
        playerActions.Player.Enable();
        lineRenderer.positionCount = 0;
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
        HeightCheck();
    }

    private void FixedUpdate()
    {
        control.Look(moveAction);
        control.Move(moveAction);
        if (rb.velocity.y < 0)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }
    }

    private void LateUpdate()
    {
        DrawSwingLine();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CustomTags.Spawn))
        {
            levelSubject.Notify();
        }
    }

    #endregion

    #region Private Methods

    private void DrawSwingLine()
    {
        if (swingJoint == null)
        {
            return;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, swingJoint.connectedAnchor);
    }

    private void HeightCheck()
    {
        if (transform.position.y < -15)
        {
            SceneManager.LoadScene(SceneNames.GameOverScene);
            ResetCursor();
        }
    }

    private void ResetCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion

    #region Public Methods

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

    #endregion
}
