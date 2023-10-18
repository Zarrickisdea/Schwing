public class StateMachine
{
    public BaseState currentState;

    public void Initialize(BaseState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }
}
