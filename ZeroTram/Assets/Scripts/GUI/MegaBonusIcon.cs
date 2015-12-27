using UnityEngine;
using System.Collections;

public class MegaBonusIcon : MonoBehaviour
{
    private IBonus _megaBonus;
    [SerializeField] private SpriteRenderer _renderer;
    private BonusTimer _timer;
    [SerializeField] private GameObject _boomPrefab;
    private Floor _floor;


    public void SetBonus(IBonus bonus, Sprite sprite)
    {
        _megaBonus = bonus;
        _renderer.sprite = sprite;
        Texture2D spriteTexture = sprite.texture;
        Vector3 spriteSize = new Vector3(spriteTexture.width / 100f, spriteTexture.height / 100f, 0);
        transform.localScale = new Vector3(1/spriteSize.x, 1/spriteSize.y);
    }

    void Start()
    {
        _floor = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor");
        _timer = MonobehaviorHandler.GetMonobeharior().GetObject<BonusTimer>("bonusTimer");
    }

	// Update is called once per frame
	void Update ()
	{
	    Vector2 mousePosition = _floor.GetCurrentMousePosition(false);
	    transform.position = mousePosition;
        if (!Input.GetMouseButton(0))
        {
            _timer.ActivateBonus(_megaBonus);
            Instantiate(_boomPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
