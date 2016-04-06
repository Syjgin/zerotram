﻿using UnityEngine;
using Assets;
using System.Collections;
using Assets.Scripts.Math;

public class RandomObject : MonoBehaviour {
    public GameObject[] go;
    private float _time;
    private const int MaxY = 50;
    private const float YCoef = 0.1f;
    private const float YCoord = 4f;
    private const float XOffset = 2.5f;
    private const float ZCoord = 10;

    void Update ()
    {
        if (!GameController.GetInstance().IsDoorsOpen())
        {
            _time -= Time.deltaTime;
            if (_time < 0)
            {
                int x = Randomizer.GetInRange(0, go.Length);
                int y = Randomizer.GetInRange(0, MaxY);
                Instantiate(go[x], new Vector3(XOffset + (YCoef * y), YCoord, ZCoord), transform.rotation);
                y = Randomizer.GetInRange(0, MaxY);
                Instantiate(go[x], new Vector3(-XOffset - (YCoef * y), YCoord, ZCoord), transform.rotation);
                _time = 0.2f;
            }
        }
	}
}
