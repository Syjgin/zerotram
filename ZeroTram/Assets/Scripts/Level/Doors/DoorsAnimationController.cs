using UnityEngine;
using System.Collections;

public class DoorsAnimationController : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private Spawner _unitSpawner;

    private bool _isGlitch;

    [SerializeField] private GameObject _glitchIndicator;
   
    private bool _isOpened;
    private float _currentStickPeriod;

	void Awake ()
	{
	    _animator = GetComponent<Animator>();
	}
    
    void Update () {
	    if (_isGlitch)
	    {
            if (_currentStickPeriod > 0)
                _currentStickPeriod -= Time.deltaTime;
            else
            {
                _currentStickPeriod = 0;
                GameController.GetInstance().KillStickPassenger();
                MonobehaviorHandler.GetMonobeharior().GetObject<DoorsTimer>("DoorsTimer").SetPaused(false);
                _isGlitch = false;
            }
        }
    }

    public void Open(bool spawn)
    {
        _glitchIndicator.SetActive(false);
        _animator.enabled = true;
        _animator.Play("doors_open");
        if (spawn)
            _unitSpawner.Spawn(gameObject);
        _isOpened = true;
        _isGlitch = false;
    }

    public void OpenAndSpawnByName(string passengerName, Spawner.TicketAdditionMode ticketMode)
    {
        _animator.enabled = true;
        _animator.Play("doors_open");
        _unitSpawner.Spawn(gameObject, passengerName, ticketMode);
        _isOpened = true;
    }

    public void Close()
    {
        _animator.Play("doors_close");
        _isOpened = false;
        _glitchIndicator.SetActive(false);
        _isGlitch = false;
    }

    public void Glitch()
    {
        if(_isGlitch)
            return;
        _animator.Play("doors_glitch");
        _isOpened = false;
        _currentStickPeriod = GameController.GetInstance().GetStickPeriod();
        _isGlitch = true;
        _glitchIndicator.SetActive(true);
    }

    public bool IsOpened()
    {
        return _isOpened;
    }
}
