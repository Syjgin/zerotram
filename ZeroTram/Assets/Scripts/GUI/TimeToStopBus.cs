using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Level;

namespace Assets
{

public class TimeToStopBus : MonoBehaviour 
{
	
	[SerializeField] private DoorsTimer _timer;

    private Text _text;

	void Start ()
	{
	    _timer = GameObject.Find("Spawner").GetComponent<DoorsTimer>();
	    _text = GetComponent<Text>();
	}

	void Update () 
    {
	    _text.text = _timer.GetCurrentRemainingTime().ToString();
	}

}

}
