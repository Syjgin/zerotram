using UnityEngine;
using Assets;
using System.Collections.Generic;

public class NewParallax : MonoBehaviour {
    public Vector2 speed = new Vector2(10, 10);

    /// <summary>
    /// Moving direction
    /// </summary>
    public Vector2 Direction = new Vector2(0.2f, -0.6f);
    public Vector2 Scale = new Vector2(0.2f, 0.2f);
    private float _mod;
    private int _x = 1;
    private const float Modifier = 2.5f;
    private const float HorizontalMovementCoef = 2f;
    // Use this for initialization
    void Start () {
        if (transform.position.x < 0)
            _x = -1;
        _mod = Modifier / Mathf.Abs(transform.position.x);
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameController.GetInstance().IsDoorsOpen())
        {
            // Movement
            Vector3 movement = new Vector3(speed.x * Direction.x * _mod * _x * HorizontalMovementCoef, speed.y * Direction.y * _mod, 0);
            Vector3 scaling = new Vector3(Scale.x * _mod, Scale.y * _mod, 0);

            movement *= Time.deltaTime;
            scaling *= Time.deltaTime;
            transform.Translate(movement);
            transform.localScale += scaling;
            if (transform.position.y / 2 < -Camera.main.ViewportToWorldPoint(new Vector3(0, 1, (transform.position - Camera.main.transform.position).z)).y)
            {
                Destroy(gameObject);
            }
        }
    }
}
