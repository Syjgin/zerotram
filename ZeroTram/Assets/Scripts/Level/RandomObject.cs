using UnityEngine;
using Assets;
using System.Collections;
using Assets.Scripts.Math;

public class RandomObject : MonoBehaviour {
    public GameObject[] go;
    private float _time;
    private const int MaxY = 50;
    private const float YCoef = 0.1f;
    private const float YCoord = 4;
    private const float XOffset = 2.5f;
    private const float ZCoord = 10;
    private const int MaxStartObstaclesCount = 10;
    private const int MinStartObstaclesCount = 5;

    void Start()
    {
        int currentObstaclesCount = Randomizer.GetInRange(MinStartObstaclesCount, MaxStartObstaclesCount);
        for (int i = 0; i < currentObstaclesCount; i++)
        {
            SpawnPairWithY(true);
        }
    }

    void Update ()
    {
        if (!GameController.GetInstance().IsDoorsOpen())
        {
            _time -= Time.deltaTime;
            if (_time < 0)
            {
                SpawnPairWithY(false);
                _time = 0.2f;
            }
        }
	}

    private void SpawnPairWithY(bool randomY)
    {
        SpawnItem(true, randomY);
        SpawnItem(false, randomY);
    }

    private void SpawnItem(bool isLeft, bool random)
    {
        int x = Randomizer.GetInRange(0, go.Length);
        float y = Randomizer.GetInRange(0, MaxY);
        float currentY = random ? Randomizer.GetBetweenOneAndMinusOne() * YCoord : YCoord;
        GameObject obj2 = (GameObject)Instantiate(go[x], new Vector3(isLeft? (-XOffset - (YCoef * y)) : (XOffset + (YCoef * y)), currentY, ZCoord), transform.rotation);
        NewParallax parallaxItem2 = obj2.GetComponent<NewParallax>();
        parallaxItem2.UpdateScale();
    }
}
