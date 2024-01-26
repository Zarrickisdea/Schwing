using UnityEngine;

public class SwingCubeViableState : SwingCubeState
{
    private float glowEffect;
    private float time;
    private bool chanceOfMadness;
    public SwingCubeViableState(SwingCube swingCube) : base(swingCube)
    {
        chanceOfMadness = false;
    }

    public override void Enter()
    {
        swingCube.CubeColor.SetColor("_EmColor", Color.blue);
        swingCube.CubeColor.SetFloat("_EffectPower", glowEffect);
        glowEffect = 0f;
        time = 0f;
        chanceOfMadness = Random.Range(0, 100) < 75;
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

    public override void ResolveTriggerStay(Collider other)
    {
        //if (other.CompareTag(CustomTags.Player))
        //{
        //    CharacterView characterView = other.GetComponent<CharacterView>();

        //    if (characterView.swingJoint != null && characterView.swingJoint.connectedBody == swingCube.Rb && chanceOfMadness)
        //    {
        //        Debug.Log("Madness");
        //        swingCube.ChangeState(swingCube.AimState);
        //    }
        //}
    }
}
