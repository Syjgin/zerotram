using Assets;

public abstract class AbstractBonus :IBonus
{
    protected float TTL;
    private bool _isActive;
    public bool IsActive()
    {
        return _isActive;
    }

    public virtual void Activate()
    {
        _isActive = true;
    }

    public abstract void Deactivate();

    public void DecrementTimer(float delta)
    {
        TTL -= delta;
        if (TTL <= 0)
            _isActive = false;
    }

    public abstract GameController.BonusTypes GetBonusType();

    public abstract void AddEffect(PassengerSM passenger);

    public abstract void RemoveEffect(PassengerSM passenger);
}