using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitlesScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect _rect;

    private bool _finished = false;

	void Update () {
        if(_finished)
            return;
	    if (_rect.verticalNormalizedPosition > 0)
	        _rect.verticalNormalizedPosition -= 0.02f*Time.deltaTime;
	    else
	    {
	        Application.LoadLevelAsync("MainMenu");
	        _finished = true;
	    }
	}
}
