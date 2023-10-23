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

    public override void ResolveTriggerEntry(Collider other)
    {
        if (other.CompareTag(CustomTags.Player))
        {
            swingCube.ChangeState(swingCube.ViableState);
        }
    }
}
