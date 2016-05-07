using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrainingHandler : MonoBehaviour
{
    [SerializeField]
    private ConductorWindow _fullConductorWindow;

    [SerializeField] private GameObject _bonusSelectWindow;
    [SerializeField] private DoorsAnimationController[] _doors;

    private const string TrainingKey = "TrainingFinished";

    private int _currentStep;

    public static bool IsTrainingFinished()
    {
        return PlayerPrefs.HasKey(TrainingKey);
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
        _currentStep++;
        ShowTrainingStep(_currentStep);
    }

    private void ShowTrainingStep(int step)
    {
        switch (step)
        {
            case 0:
                _bonusSelectWindow.SetActive(false);
                _fullConductorWindow.gameObject.SetActive(true);
                _fullConductorWindow.DisplayText(StringResources.GetLocalizedString("Training1"), false);
                break;
            case 1:
                _fullConductorWindow.DisplayTextWithImage(StringResources.GetLocalizedString("Training2"), Resources.Load<Sprite>("Sprites/training/training1"), false);
                break;
            case 2:
                _fullConductorWindow.DisplayText(StringResources.GetLocalizedString("Training3"), true);
                break;
            case 3:
                int index = Randomizer.GetInRange(0, _doors.Length);
                _doors[index].OpenAndSpawnByName("gnome");
                break;
        }
    }
}
