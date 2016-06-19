using UnityEngine;
using System.Collections;

public class UnknownDrop : MonoBehaviour
{
    [SerializeField] private Sprite _pieSprite;
    [SerializeField] private Sprite _clewSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public IBonus Bonus;
    private float _destroyPeriod;
    private float _currentPeriod;

    private const int WaitTimeForTraining = 1;

	void Awake ()
	{
	    _currentPeriod = 0;
	    _destroyPeriod = ConfigReader.GetConfig().GetField("bonuses").GetField("DestroyPeriod").n;
	}
	
	void FixedUpdate ()
	{
	    _currentPeriod += Time.fixedDeltaTime;
	    if (_currentPeriod > _destroyPeriod && TrainingHandler.IsTrainingFinished())
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
        } else if (bonus is ClewBonus)
        {
            _spriteRenderer.sprite = _clewSprite;
        }
    }

    void OnMouseDown()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (!TrainingHandler.IsTrainingFinished())
        {
            if(_currentPeriod < WaitTimeForTraining)
                return;
        }
        MonobehaviorHandler.GetMonobeharior()
                .GetObject<BonusTimer>("bonusTimer").ActivateDrop(this);
    }
}
