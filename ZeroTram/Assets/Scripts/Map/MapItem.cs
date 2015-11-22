using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapItem : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _clickHandler;
    [SerializeField] private string _levelId;

    void Start()
    {
        gameObject.SetActive(MapManager.GetInstance().IsStationOpened(_levelId));
    }

    void OnMouseDown()
    {
        MapManager.GetInstance().SetCurrentStation(_levelId);
        Application.LoadLevelAsync("Main");
    }
}
