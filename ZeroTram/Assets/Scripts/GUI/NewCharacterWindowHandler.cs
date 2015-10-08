using UnityEngine;

public class NewCharacterWindowHandler
{
    private static NewCharacterWindowHandler _instance;

    public static NewCharacterWindow GetWindow()
    {
        if(_instance == null)
            _instance = new NewCharacterWindowHandler();
        return _instance._window;
    }

    private NewCharacterWindow _window;

    private NewCharacterWindowHandler()
    {
        _window = GameObject.Find("Spawner").GetComponent<NewCharacterWindow>();
    }
}
