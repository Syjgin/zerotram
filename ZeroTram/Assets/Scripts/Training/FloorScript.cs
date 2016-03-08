using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorScript : MonoBehaviour
{
    [SerializeField]
    private ConductorSM _hero;
    [SerializeField]
    private List<BoxCollider2D> _doors;
    [SerializeField]
    private BoxCollider2D _centralWayout;
    [SerializeField]
    private PolygonCollider2D _polygonCollider2D;

    private bool _pause;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!_pause)
        {

        }
    }

    private bool IsHeroNearCentralWayout()
    {
        return IsHeroNearWayout(_centralWayout, true);
    }

    private bool IsHeroNearWayout(Collider2D wayout, bool central)
    {
        if (_hero == null)
            return false;
        Vector2 position = _hero.transform.position;
        if (central)
            position.y -= 0.7f;
        return wayout.OverlapPoint(position);
    }

    public void ChangeWayoutSquare(float coef)
    {
        _centralWayout.size *= coef;
    }
}
