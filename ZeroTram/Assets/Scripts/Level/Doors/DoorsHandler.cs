using UnityEngine;

public class DoorsHandler
{
    private static DoorsHandler _instance;

    public static DoorsTimer GetTimer()
    {
        if(_instance == null || _instance._timer == null)
            _instance = new DoorsHandler();
        return _instance._timer;
    }

    private DoorsTimer _timer;

    private DoorsHandler()
    {
        _timer = GameObject.Find("Spawner").GetComponent<DoorsTimer>();
    }
}
