using UnityEngine;
using UnityEngine.UI;

public class SwingCube : MonoBehaviour
{
    [SerializeField] private MeshRenderer cubeMeshRenderer;
    [SerializeField] private SphereCollider cubeSphereCollider;

    private StateMachine cubeStateMachine;

    public SwingCubeStartState StartState;
    public SwingCubeViableState ViableState;
    public SwingCubeAimState AimState;

    public Material CubeColor
    {
        get
        {
            return cubeMeshRenderer.material;
        }
    }

    public BaseState CurrentState
    {
        get
        {
            return cubeStateMachine.currentState;
        }
    }

    private void Awake()
    {
        cubeStateMachine = new StateMachine();
        StartState = new SwingCubeStartState(this);
        ViableState = new SwingCubeViableState(this);
        AimState = new SwingCubeAimState(this);
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
            cubeStateMachine.currentState.ResolveTriggers(other);
        }
    }

    public void ChangeState(SwingCubeState state)
    {
        cubeStateMachine.ChangeState(state);
    }
}
