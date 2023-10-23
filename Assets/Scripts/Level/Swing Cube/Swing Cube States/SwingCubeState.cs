public class SwingCubeState : BaseState
{
    protected SwingCube swingCube;
    public SwingCubeState(SwingCube swingCube)
    {
        this.swingCube = swingCube;
    }

    public virtual void NotifyState(SwingCubeState state)
    {

    }
}
