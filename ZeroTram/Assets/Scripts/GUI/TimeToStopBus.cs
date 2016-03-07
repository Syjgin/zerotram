using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Level;

namespace Assets
{

public class TimeToStopBus : MonoBehaviour 
{
	
	[SerializeField] private DoorsTimer _timer;
    [SerializeField] private Text _text;

    private const int FullZeroCount = 4;

	void Start ()
	{
	    _timer = GameObject.Find("DoorsTimer").GetComponent<DoorsTimer>();
	}

	void Update ()
	{
	    int remainTime = _timer.GetCurrentRemainingTime();
	    int zeroCount = FullZeroCount - remainTime.ToString().Length;
	    string result = string.Empty;
	    for (int i = 0; i < zeroCount; i++)
	    {
	        result += "0";
	    }
	    result += remainTime;
	    result = result.Insert(2, ":");
            _text.text = result;

    }

}

}
