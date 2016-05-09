using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrainingHandler : MonoBehaviour
{
    [SerializeField]
    private ConductorWindow _fullConductorWindow;
    [SerializeField]
    private ConductorWindow _shortConductorWindow;

    [SerializeField] private GameObject _bonusSelectWindow;
    [SerializeField] private DoorsAnimationController[] _doors;
    [SerializeField] private GameObject _doorsTimer;
    [SerializeField]
    private GameObject _ticketsCounter;
    [SerializeField]
    private GameObject _haresCounter;
    [SerializeField]
    private GameObject _killedCounter;
    [SerializeField]
    private GameObject _lifes;

    private const string TrainingKey = "TrainingFinished";

    private int _currentStep;
    private bool _isRefreshInProgress;

    public static bool IsTrainingFinished()
    {
        return PlayerPrefs.HasKey(TrainingKey);
    }

    public bool IsDoorsTimerEnabled()
    {
        return _currentStep > 6;
    }

	// Use this for initialization
	void Start () {
        ShowTrainingStep(0);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void ShowNext()
    {
        if(_isRefreshInProgress)
            return;
        _isRefreshInProgress = true;
        _currentStep++;
        ShowTrainingStep(_currentStep);
    }

    private void ShowTrainingStep(int step)
    {
        switch (step)
        {
            case 0:
                _doorsTimer.SetActive(false);
                _ticketsCounter.SetActive(false);
                _haresCounter.SetActive(false);
                _killedCounter.SetActive(false);
                _lifes.SetActive(false);
                _bonusSelectWindow.SetActive(false);
                Time.timeScale = 0;
                _fullConductorWindow.DisplayText(StringResources.GetLocalizedString("Training1"), false);
                break;
            case 1:
                _fullConductorWindow.DisplayTextWithImage(StringResources.GetLocalizedString("Training2"), Resources.Load<Sprite>("Sprites/training/training1"), false);
                break;
            case 2:
                _fullConductorWindow.DisplayText(StringResources.GetLocalizedString("Training3"), true);
                break;
            case 3:
                Time.timeScale = 1;
                int index = Randomizer.GetInRange(0, _doors.Length);
                _doors[index].OpenAndSpawnByName("gnome", Spawner.TicketAdditionMode.WithTicket);
                StartCoroutine(WaitAndMoveNext(2));
                break;
            case 4:
                Time.timeScale = 0;
                _shortConductorWindow.DisplayText(StringResources.GetLocalizedString("Training4"), true);
                GameObject gnomeObject = GameObject.Find("gnome(Clone)");
                Gnome passenger = gnomeObject.GetComponent<Gnome>();
                passenger.EnableTrainingClick();
                break;
            case 5:
                Time.timeScale = 1;
                _ticketsCounter.SetActive(true);
                break;
            case 6:
                Time.timeScale = 0;
                _doorsTimer.SetActive(true);
                _shortConductorWindow.DisplayText(StringResources.GetLocalizedString("Training5"), true);
                break;
            case 7:
                Time.timeScale = 1;
                break;
        }
        _isRefreshInProgress = false;
    }

    private IEnumerator WaitAndMoveNext(float amount)
    {
        yield return new WaitForSeconds(amount);
        ShowNext();
    }
}
