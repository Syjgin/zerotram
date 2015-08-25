using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private MovableObject _hero;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnMouseDown()
    {
        Vector2 pos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _hero.SetTarget(pos);
    }
}
