using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ServerConfigLoader : MonoBehaviour
{
    [SerializeField] private Client _serverClient;
    [SerializeField] private String _scene2load = "main";

	// Use this for initialization
	void Start ()
	{
		PlayerPrefs.DeleteAll ();
	    if (ConfigReader.IsLocalConfigAvailable())
	    {
	        _serverClient.CheckConfig((result) =>
	        {
	            if (result.HasField("error"))
	            {
	                Debug.Log("error check config");
					_serverClient.DisableClient();
					LoadNextScene ();
	            }
	            else
	            {
	                String version = result.GetField("configVersion").str;
	                String localversion = ConfigReader.ConfigVersion();
                    if (localversion.Equals(version))
	                {
						ContinueLoading();
	                }
	                else
	                {
                        LoadNewConfig();	                    
	                }
	            }
	        });
	    }
	    else
	    {
	        LoadNewConfig();
	    }	    
	}

    private void LoadNewConfig()
    {
        _serverClient.LoadConfig((result) =>
        {
            if (result.HasField("error"))
            {
				Debug.LogError(result.GetField ("error"));
            }
            else
            {
                ConfigReader.SetConfig(result);
				ContinueLoading();
            }
        });
    }

    private void ContinueLoading()
    {
		if (_serverClient.IsUserIdSaved()) {
			_serverClient.AuthorizeUser((result) => {
				if (result.HasField("error"))
				{
					Debug.LogError(result.GetField ("error"));
					_serverClient.DisableClient();
					LoadNextScene ();
				} else {
					_serverClient.UpdateToken (result.GetField ("token").str);
					LoadNextScene ();
				}
			});
		} else {
			String username = System.Guid.NewGuid().ToString();
			registerWithUsername (username);
		}
    }

	private void registerWithUsername(String username) {
		_serverClient.RegisterUser (username, (result) => {
			if (result.HasField("error"))
			{
				Debug.LogError(result.GetField ("error"));
				_serverClient.DisableClient();
				LoadNextScene ();
			}
			else
			{
				if(result.HasField ("freeuuid")) {
					Debug.Log ("this user id already exists. Trying with different one");
					registerWithUsername (result.GetField ("freeuuid").str);
				} else {
					if(result.HasField ("token"))	{
						_serverClient.UpdateToken (result.GetField ("token").str);
						LoadNextScene();
					}
				}
			}	
		});
	}

	private void LoadNextScene() {
		SceneManager.LoadScene (_scene2load);
	}
}
