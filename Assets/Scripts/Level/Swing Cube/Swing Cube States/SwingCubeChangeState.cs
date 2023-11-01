using UnityEngine;

public class SwingCubeChangeState : SwingCubeState
{
    private Vector3 newPosition;
    private Vector3 oldPosition;
    private float initialXDuration;
    private float zDuration;
    private float yDuration;
    private float finalXDuration;
    private float tempX;

    public SwingCubeChangeState(SwingCube swingCube) : base(swingCube)
    {
    }

    public void SetChangeParameters(float duration, Vector3 newPosition, float xSeperation)
    {
        stateTimer = duration;
        this.newPosition = newPosition;

        initialXDuration = duration * 0.25f;
        zDuration = duration * 0.25f;
        yDuration = duration * 0.25f;
        finalXDuration = duration * 0.25f;
        tempX = newPosition.x + (newPosition.x > 0 ? xSeperation : -xSeperation);

        oldPosition = swingCube.transform.position;
    }

    public override void Enter()
    {
        swingCube.CubeColor.SetColor("_EmColor", Color.white);
        swingCube.CubeColor.SetFloat("_EffectPower", 4f);
    }

    public override void UpdateLogic()
    {
        if (stateTimer > 0f)
        {
            float progress = 1f - (stateTimer / (initialXDuration + zDuration + yDuration + finalXDuration));

            if (progress <= 0.25f)
            {
                float t = progress / 0.25f;
                float newX = Mathf.Lerp(oldPosition.x, tempX, t);
                swingCube.transform.position = new Vector3(newX, oldPosition.y, oldPosition.z);
            }
            else if (progress <= 0.5f)
            {
                float t = (progress - 0.25f) / 0.25f;
                float newZ = Mathf.Lerp(oldPosition.z, newPosition.z, t);
                swingCube.transform.position = new Vector3(swingCube.transform.position.x, oldPosition.y, newZ);
            }
            else if (progress <= 0.75f)
            {
                float t = (progress - 0.5f) / 0.25f;
                float newY = Mathf.Lerp(oldPosition.y, newPosition.y, t);
                swingCube.transform.position = new Vector3(swingCube.transform.position.x, newY, swingCube.transform.position.z);
            }
            else
            {
                float t = (progress - 0.75f) / 0.25f;
                float newX = Mathf.Lerp(tempX, newPosition.x, t);
                swingCube.transform.position = new Vector3(newX, newPosition.y, newPosition.z);
            }

            stateTimer -= Time.deltaTime;
        }
        else
        {
            swingCube.ChangeState(swingCube.StartState);
        }
    }
}
