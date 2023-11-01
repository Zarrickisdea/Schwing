using UnityEngine;

public class SwingCubeViableState : SwingCubeState
{
    private float glowEffect;
    private float time;
    public SwingCubeViableState(SwingCube swingCube) : base(swingCube)
    {
    }

    public override void Enter()
    {
        swingCube.CubeColor.SetColor("_EmColor", Color.blue);
        swingCube.CubeColor.SetFloat("_EffectPower", glowEffect);
        glowEffect = 0f;
        time = 0f;
    }

    public override void UpdateLogic()
    {
        time += Time.deltaTime;
        glowEffect = Mathf.Lerp(0f, 3f, time);

        swingCube.CubeColor.SetFloat("_EffectPower", glowEffect);
    }

    public override void ResolveTriggerExit(Collider other)
    {
        if (other.CompareTag(CustomTags.Player))
        {
            swingCube.ChangeState(swingCube.StartState);
        }
    }
}
