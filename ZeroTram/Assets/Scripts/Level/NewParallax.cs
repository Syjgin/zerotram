using UnityEngine;
using Assets;
using System.Collections.Generic;

public class NewParallax : MonoBehaviour {
    public Vector2 speed = new Vector2(10, 10);

    /// <summary>
    /// Moving direction
    /// </summary>
    public Vector2 direction = new Vector2(0.2f, -0.6f);
    public Vector2 scale = new Vector2(0.2f, 0.2f);
    private float mod;
    private int x = 1;
    // Use this for initialization
    void Start () {
        if (transform.position.x < 0)
            x = -1;
        mod = 2.5f / Mathf.Abs(transform.position.x) * x;
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameController.GetInstance().IsDoorsOpen())
        {
            // Movement
            Vector3 movement = new Vector3(speed.x * direction.x * mod, speed.y * direction.y * mod, 0);
            Vector3 scaling = new Vector3(scale.x * mod, scale.y * mod, 0);

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
