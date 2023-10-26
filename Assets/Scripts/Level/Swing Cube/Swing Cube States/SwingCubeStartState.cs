using UnityEngine;
public class SwingCubeStartState : SwingCubeState
{
    public SwingCubeStartState(SwingCube swingCube) : base(swingCube)
    {
    }

    public override void Enter()
    {
        swingCube.CubeColor.SetColor("_EmColor", Color.red);  
        swingCube.CubeColor.SetFloat("_EffectPower", 0);
    }

    public override void ResolveTriggerEntry(Collider other)
    {
        if (other.CompareTag(CustomTags.Player))
        {
            swingCube.ChangeState(swingCube.ViableState);
        }
    }

    public override void ResolveTriggerStay(Collider other)
    {
        if (other.CompareTag(CustomTags.Player))
        {
            swingCube.ChangeState(swingCube.ViableState);
        }
    }
}
