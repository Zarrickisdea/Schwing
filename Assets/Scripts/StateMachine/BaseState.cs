using UnityEngine;

public abstract class BaseState
{
    protected float stateTimer;
    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void UpdateLogic()
    {
    }

    public virtual void UpdatePhysics()
    {
    }

    public virtual void UpdateGraphics()
    {
    }

    public virtual void ResolveTriggerEntry(Collider other)
    {
    }

    public virtual void ResolveTriggerExit(Collider other)
    {
    }
}

