using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapItem : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _clickHandler;
    [SerializeField] private string _levelId;
    [SerializeField] private UnityEngine.UI.Text _nameText;

    void Start()
    {
        gameObject.SetActive(MapManager.GetInstance().IsStationOpened(_levelId));
        if (gameObject.activeInHierarchy)
        {
            _nameText.text = MapManager.GetStationInfo(_levelId).Name;
        }
    }

    void OnMouseDown()
    {
        MapManager.GetInstance().SetCurrentStation(_levelId);
        Application.LoadLevelAsync("Main");
    }
}
