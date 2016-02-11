using UnityEngine;
using Assets;
using System.Collections;

public class RandomObject : MonoBehaviour {
    public GameObject[] go;
    private float time = 0f;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameController.GetInstance().IsDoorsOpen())
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                int x = Random.Range(0, go.Length);
                int y = Random.Range(0, 36);
                Instantiate(go[x], new Vector3(2.5f + (0.1f * y), 3.5f), transform.rotation);
                y = Random.Range(0, 36);
                Instantiate(go[x], new Vector3(-2.5f - (0.1f * y), 3.5f), transform.rotation);
                time = 1f;
            }
        }
	}
}
