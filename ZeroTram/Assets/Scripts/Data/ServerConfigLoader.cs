using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Client;
using UnityEngine.SceneManagement;

public class ServerConfigLoader : MonoBehaviour
{
    [SerializeField] private Client _serverClient;
    [SerializeField] private String _scene2load = "main";

	// Use this for initialization
	void Start ()
	{
	    String currentConfig = ConfigReader.GetConfig().ToString();
	    if (ConfigReader.IsLocalConfigAvailable())
	    {
	        _serverClient.CheckConfig((result) =>
	        {
	            if (result.HasField("error"))
	            {
	                Debug.Log("error check config");
	                LoadScene();
	            }
	            else
	            {
	                String version = result.GetField("configVersion").str;
	                String localversion = ConfigReader.ConfigVersion();
                    if (localversion.Equals(version))
	                {
	                    LoadScene();
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
                Debug.Log("error loading config");
                LoadScene();
            }
            else
            {
                ConfigReader.SetConfig(result);
                LoadScene();
            }
        });
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(_scene2load);
    }
}
