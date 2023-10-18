using UnityEngine;

public class SwingCubeViableState : SwingCubeState
{
    public SwingCubeViableState(SwingCube swingCube) : base(swingCube)
    {
    }

    public override void Enter()
    {
        swingCube.CubeColor.color = Color.blue;
    }

    public override void NotifyState(SwingCubeState state)
    {

    }
}
