using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ArrowRenderer : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private SpriteRenderer _renderer;
    
    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

	void Update ()
	{
        bool onBottomPartOfScreen = _camera.WorldToScreenPoint(gameObject.transform.parent.position).y >= Screen.height * 0.5f;
        _renderer.flipY = !onBottomPartOfScreen;
        transform.localPosition = onBottomPartOfScreen ? new Vector3(1, -1.4f, -8) : new Vector3(1, 1.4f, -8);
    }
}
