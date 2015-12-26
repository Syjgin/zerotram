using System;
using UnityEngine;
using System.Collections;

public class Bench : MonoBehaviour
{
    public PassengerSM CurrentPassenger;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public String CurrentPassengerClassName()
    {
        if (CurrentPassenger == null)
            return String.Empty;
        return CurrentPassenger.GetClassName();
    }
}
