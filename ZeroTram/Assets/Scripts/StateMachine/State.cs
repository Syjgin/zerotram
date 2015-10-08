
public class State
{
    protected virtual void OnStart()
    {
    }

    protected virtual void OnEnd()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnFixedUpdate() { }

    public State(StateMachine machine)
    {
        Parent = machine;
    }

    private bool _isEnabled;
    protected StateMachine Parent;

    public bool IsEnabled()
    {
        return _isEnabled;
    }

    public virtual bool IsTransitionAllowed()
    {
        return true;
    }

    public void SetEnabled(bool enabled)
    {
        bool wasEnabled = _isEnabled;
        _isEnabled = enabled;
        if (_isEnabled && !wasEnabled)
        {
            OnStart();
        }
        if(!_isEnabled && wasEnabled)
        {
            OnEnd();
        }
    }
}
