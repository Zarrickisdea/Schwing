using UnityEngine;
public class SwingCubeStartState : SwingCubeState
{
    public SwingCubeStartState(SwingCube swingCube) : base(swingCube)
    {
    }

    public override void Enter()
    {
        swingCube.CubeColor.color = Color.red;
    }

    public override void NotifyState(SwingCubeState state)
    {

    }
}
