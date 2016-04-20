using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResolutionAdapter : MonoBehaviour {
	[SerializeField]
	Image _bottomPanel;

	[SerializeField]
	Image _topPanel;

	[SerializeField]
	PolygonCollider2D _floor;

	[SerializeField]
	Camera _camera;

	// Use this for initialization
	void Start () {
		float aspect = ((float)Screen.width / (float)Screen.height) - 1;
		if(aspect > 0) {
			float reverseAspect = 0.55f*((float)Screen.height / (float)Screen.width);
			_bottomPanel.rectTransform.transform.localScale = new Vector3 (1, reverseAspect, 1);
			_topPanel.rectTransform.transform.localScale = new Vector3 (1, reverseAspect, 1);
		}
	}
}
