using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlesScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect _rect;
    [SerializeField] private Button _exit;

    private bool _finished = false;

    void Start()
    {
        _exit.onClick.AddListener(Exit);
    }

    void Exit()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        _finished = true;
    }

	void Update () {
        if(_finished)
            return;
	    if (_rect.verticalNormalizedPosition > 0)
	        _rect.verticalNormalizedPosition -= 0.02f*Time.deltaTime;
	    else
	    {
            Exit();
	    }
	}
}
