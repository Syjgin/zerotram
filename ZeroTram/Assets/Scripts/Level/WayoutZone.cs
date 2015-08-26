using Assets;
using UnityEngine;
using System.Collections;

public class WayoutZone : MonoBehaviour
{
    private float _radius;
    private Hero _hero;

	void Start ()
	{
	    BoxCollider2D collider = GetComponent<BoxCollider2D>();
	    float size = collider.bounds.size.x;
        _radius = Mathf.Sqrt(2*size*size)/2;
	    _hero = GameObject.Find("hero").GetComponent<Hero>();
	}

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 transform2d = transform.position;
            float distance = (transform2d - hit.centroid).sqrMagnitude;
            Vector2 heroTransform2D = _hero.transform.position;
            float heroDistance = (heroTransform2D - hit.centroid).sqrMagnitude;
            if (distance < _radius && heroDistance < _radius)
            {
                if (!_hero.IsUnderAttack())
                {
                    _hero.Kick();
                }
            }
        }
    }
}
