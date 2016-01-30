using UnityEngine;
using System.Collections;

public class FloorCollisionDetector : MonoBehaviour
{
    [SerializeField] private Floor _floor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        _floor.OnMouseDown();
    }
}
