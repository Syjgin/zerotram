using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour
{
    [SerializeField] private List<ScrollingScript> _layers;

    private const int Decrement = 2;

	void Start ()
	{
	    Vector2 baseVelocity = new Vector2(10, 10);
	    foreach (var scrollingScript in _layers)
	    {
	        scrollingScript.SetSpeed(baseVelocity);
	        baseVelocity.x -= Decrement;
	        baseVelocity.y -= Decrement;
	    }
	}

    public void SetEnabled(bool isenabled)
    {
        foreach (var scrollingScript in _layers)
        {
            scrollingScript.SetEnabled(isenabled);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
