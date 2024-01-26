using UnityEngine;

public class SwingCubeAimState : SwingCubeState
{
    public SwingCubeAimState(SwingCube swingCube) : base(swingCube)
    {
    }

    public override void Enter()
    {
        swingCube.CubeColor.SetFloat("_EffectPower", 3f);
        swingCube.CubeColor.SetColor("_EmColor", Color.green);

        swingCube.Rb.MovePosition(swingCube.transform.position + Vector3.forward * 200f);
    }
}
