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

    public override void ResolveTriggerExit(Collider other)
    {
        if (other.CompareTag(CustomTags.Player))
        {
            swingCube.ChangeState(swingCube.StartState);
        }
    }
}
