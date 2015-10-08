using UnityEngine;

public class FloorHandler
{
    private static FloorHandler _instance;

    public static Floor GetFloor()
    {
        if(_instance == null || _instance._floor == null)
            _instance = new FloorHandler();
        return _instance._floor;
    }

    private Floor _floor;

    private FloorHandler()
    {
        _floor = GameObject.Find("Floor").GetComponent<Floor>();
    }
}
