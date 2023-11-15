using UnityEngine;

public class SwingCube : MonoBehaviour
{
    [SerializeField] private MeshRenderer cubeMeshRenderer;

    private StateMachine cubeStateMachine;
    private Rigidbody rb;

    public SwingCubeStartState StartState;
    public SwingCubeViableState ViableState;
    public SwingCubeChangeState ChangingState;
    public SwingCubeAimState AimState;

    public Material CubeColor
    {
        get
        {
            return cubeMeshRenderer.material;
        }
    }

    public Rigidbody Rb
    {
        get
        {
            return rb;
        }
    }

    private void Awake()
    {
        cubeStateMachine = new StateMachine();
        StartState = new SwingCubeStartState(this);
        ViableState = new SwingCubeViableState(this);
        ChangingState = new SwingCubeChangeState(this);
        AimState = new SwingCubeAimState(this);

        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (cubeStateMachine.currentState != null)
        {
            cubeStateMachine.currentState.UpdateLogic();
        }
    }

    private void FixedUpdate()
    {
        if (cubeStateMachine.currentState != null)
        {
            cubeStateMachine.currentState.UpdatePhysics();
        }
    }

    private void LateUpdate()
    {
        if (cubeStateMachine.currentState != null)
        {
            cubeStateMachine.currentState.UpdateGraphics();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cubeStateMachine.currentState != null)
        {
            cubeStateMachine.currentState.ResolveTriggerEntry(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (cubeStateMachine.currentState != null)
        {
            cubeStateMachine.currentState.ResolveTriggerExit(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (cubeStateMachine.currentState != null)
        {
            cubeStateMachine.currentState.ResolveTriggerStay(other);
        }
    }

    public void ChangeState(SwingCubeState state)
    {
        cubeStateMachine.ChangeState(state);
    }
}
