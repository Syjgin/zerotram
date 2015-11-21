using UnityEngine;
using System.Collections;

public class MapItem : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _clickHandler;
    [SerializeField] private string _levelId;

    void Start()
    {
        enabled = MapManager.GetInstance().IsStationOpened(_levelId);
    }

    void OnMouseDown()
    {
        LevelManager.SetCurrentLevel(_levelId);
        Application.LoadLevelAsync("Main");
    }
}
