using UnityEngine;

public class AudioController
{
    private static AudioController _instance;

    public static AudioPlayer GetPlayer()
    {
        if(_instance == null || _instance._player == null)
            _instance = new AudioController();
        return _instance._player;
    }

    private AudioPlayer _player;

    private AudioController()
    {
        _player = GameObject.Find("AudioPlayer").GetComponent<AudioPlayer>();
    }
}
