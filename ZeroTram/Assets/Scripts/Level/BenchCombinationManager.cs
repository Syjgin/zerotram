using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BenchCombinationManager : MonoBehaviour
{
	[SerializeField] private Client _client;

	private bool _isInitialized;

    public void Start()
    {
		_client.StartCombination ((JSONObject result) => {
			if(result.HasField ("result") && result.GetField ("result").str == "true") {
				_isInitialized = true;	
			}
		});
    }
		
    public void CalculateCurrent()
    {
		if (!_isInitialized)
			return;
		List<string> currentNames = GameController.GetInstance ().GetSitPassengers ();
		if(currentNames.Count > 0) {
			_client.SendCombination ((JSONObject result) => {
				Debug.Log (result.str);
			}, currentNames);	
		}
    }
}