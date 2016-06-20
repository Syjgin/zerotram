using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BenchCombinationManager : MonoBehaviour
{
	[SerializeField] private Client _client;
    [SerializeField] private MyMessageScript _messagesManager;

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
			_client.SendCombination ((JSONObject result) =>
			{
			    if (!result.HasField("error"))
			    {
			        float reward = result.GetField("reward").n;
                    string name = result.GetField("name").str;
			        string combinationName = StringResources.GetLocalizedString(name);
			        string message = String.Format("{0}!\n{1}:{2}", combinationName, StringResources.GetLocalizedString("reward"), reward);
                    _messagesManager.AddMessage(message);
                }
				//Debug.Log (result.str);
			}, currentNames);	
		}
    }
}