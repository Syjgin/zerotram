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
    [SerializeField]
    private GameObject _arrowPrefab;

    [SerializeField] private GameObject _centralWayout;
    [SerializeField]
    private GameObject _centralWayoutSprite;

    [SerializeField] private DoorsTimer _doorsTimerController;

    private GameObject _activeArrow;

    private const string TrainingKey = "TrainingFinished";

    private int _currentStep;
    private bool _isRefreshInProgress;
    private bool _isPassengerClickAllowed;
    private bool _isFlyAwayEnabled;

    public static bool IsTrainingFinished()
    {
        return PlayerPrefs.HasKey(TrainingKey);
    }
    
    public bool IsPassengerClickAllowed()
    {
        return _isPassengerClickAllowed;
    }

    public bool IsFlyAwayEnabled()
    {
        return _isFlyAwayEnabled;
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
                GameController.GetInstance().SetDoorsOpen(true);
                _doorsTimer.SetActive(false);
                _ticketsCounter.SetActive(false);
                _haresCounter.SetActive(false);
                _killedCounter.SetActive(false);
                _lifes.SetActive(false);
                _bonusSelectWindow.SetActive(false);
                _isPassengerClickAllowed = false;
                _isFlyAwayEnabled = false;
                _centralWayout.SetActive(false);
                _centralWayoutSprite.SetActive(false);
                Time.timeScale = 0;
                _doorsTimerController.SetMoveAndStopDuration(3, 1);
                _doorsTimerController.SetMovementLocked(true);
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
                SpawnPassengerFromRandomDoor("gnome", Spawner.TicketAdditionMode.WithTicket);
                StartCoroutine(WaitAndMoveNext(2));
                break;
            case 4:
                Time.timeScale = 0;
                _shortConductorWindow.DisplayText(StringResources.GetLocalizedString("Training4"), true);
                GameObject gnomeObject = GameObject.Find("gnome(Clone)");
                Gnome passenger = gnomeObject.GetComponent<Gnome>();
                DisplayArrowForPassenger(passenger);
                _isPassengerClickAllowed = true;
                break;
            case 5:
                Time.timeScale = 1;
                _ticketsCounter.SetActive(true);
                break;
            case 6:
                Time.timeScale = 0;
                Destroy(_activeArrow);
                _doorsTimer.SetActive(true);
                _shortConductorWindow.DisplayText(StringResources.GetLocalizedString("Training5"), true);
                break;
            case 7:
                Time.timeScale = 1;
                _doorsTimerController.SetMovementLocked(false);
                StartCoroutine(WaitAndMoveNext(2));
                break;
            case 8:
                SpawnPassengerFromRandomDoor("bird", Spawner.TicketAdditionMode.WithoutTicket);
                GameObject bird = GameObject.Find("bird(Clone)");
                Bird birdPassenger = bird.GetComponent<Bird>();
                birdPassenger.SetAttackEnabled(false);
                DisplayArrowForPassenger(birdPassenger);
                _doorsTimerController.SetMovementLocked(true);
                break;
            case 9:
                Time.timeScale = 0;
                Destroy(_activeArrow);
                _doorsTimer.SetActive(true);
                _shortConductorWindow.DisplayText(StringResources.GetLocalizedString("Training6"), true);
                break;
            case 10:
                _centralWayout.SetActive(true);
                _centralWayoutSprite.SetActive(true);
                DisplayArrow(_centralWayout);
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

    private void DisplayArrowForPassenger(PassengerSM passenger)
    {
        DisplayArrow(passenger.gameObject);
        passenger.EnableTrainingClick();
    }

    private void DisplayArrow(GameObject go)
    {
        GameObject arrow = (GameObject)Instantiate(_arrowPrefab, go.gameObject.transform.position, Quaternion.identity);
        arrow.transform.parent = go.gameObject.transform;
        arrow.transform.localPosition = new Vector3(0, 1.7f, -8);
        _activeArrow = arrow;
    }

    private void SpawnPassengerFromRandomDoor(string passengerName, Spawner.TicketAdditionMode mode)
    {
        int index = Randomizer.GetInRange(0, _doors.Length);
        _doors[index].OpenAndSpawnByName(passengerName, mode);
    }
}
