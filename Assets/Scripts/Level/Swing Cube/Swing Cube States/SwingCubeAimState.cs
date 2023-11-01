using UnityEngine;

public class SwingCubeAimState : SwingCubeState
{
    public SwingCubeAimState(SwingCube swingCube) : base(swingCube)
    {
    }

    public override void Enter()
    {
        swingCube.CubeColor.color = Color.green;
    }
}
