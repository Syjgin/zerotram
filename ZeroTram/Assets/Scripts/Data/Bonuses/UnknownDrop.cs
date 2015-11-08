using UnityEngine;
using System.Collections;

public class UnknownDrop : MonoBehaviour
{
    public IBonus Bonus;
    private const int DestroyPeriod = 10;//TODO: move to config
    private float _currentPeriod;

	void Start ()
	{
	    _currentPeriod = 0;
	}
	
	void FixedUpdate ()
	{
	    _currentPeriod += Time.fixedDeltaTime;
	    if (_currentPeriod > DestroyPeriod)
	    {
	        Destroy(gameObject);
	    }
	}

    public void InitWithBonus(IBonus bonus)
    {
        Bonus = bonus;
    }

    void OnMouseDown()
    {
        MonobehaviorHandler.GetMonobeharior()
                .GetObject<BonusTimer>("bonusTimer").ActivateDrop(this);
    }
}
