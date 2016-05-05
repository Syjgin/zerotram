using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrainingHandler : MonoBehaviour
{
    [SerializeField]
    private ConductorWindow _fullConductorWindow;

    private const string TrainingKey = "TrainingFinished";

    private int _currentStep;

    public bool IsTrainingFinished()
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
                _fullConductorWindow.gameObject.SetActive(true);
                _fullConductorWindow.DisplayText(StringResources.GetLocalizedString("Training1"), false);
                break;
            case 1:
                _fullConductorWindow.DisplayText(StringResources.GetLocalizedString("Training2"), true);
                break;
        }
    }
}
