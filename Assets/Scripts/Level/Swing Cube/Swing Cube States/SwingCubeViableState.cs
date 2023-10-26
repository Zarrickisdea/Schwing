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
        glowEffect = -1f;
        time = 0f;
    }

    public override void UpdateLogic()
    {
        time += Time.deltaTime;
        glowEffect = Mathf.Lerp(-1f, 1f, time);

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
