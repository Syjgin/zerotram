using UnityEngine;
using System.Collections;

public class UnknownDrop : MonoBehaviour
{
    [SerializeField] private Sprite _pieSprite;
    private SpriteRenderer _spriteRenderer;

    public IBonus Bonus;
    private float _destroyPeriod;
    private float _currentPeriod;

	void Awake ()
	{
	    _currentPeriod = 0;
	    _destroyPeriod = ConfigReader.GetConfig().GetField("bonuses").GetField("DestroyPeriod").n;
	    _spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void FixedUpdate ()
	{
	    _currentPeriod += Time.fixedDeltaTime;
	    if (_currentPeriod > _destroyPeriod)
	    {
	        Destroy(gameObject);
	    }
	}

    public void InitWithBonus(IBonus bonus)
    {
        Bonus = bonus;
        if (bonus is HealBonus)
        {
            _spriteRenderer.sprite = _pieSprite;
        }
    }

    void OnMouseDown()
    {
        MonobehaviorHandler.GetMonobeharior()
                .GetObject<BonusTimer>("bonusTimer").ActivateDrop(this);
    }
}
