using System.Collections.Generic;
using Assets;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> unitPrefabs;
    private bool _isFirstStation;
    private const int MaxPassengers = 20;

    public void Spawn(GameObject spawnPoint)
    {
        int minCount = 0;
        if (_isFirstStation)
        {
            _isFirstStation = false;
            minCount = 1;
        }
        int maxCount = GameController.GetInstance().GetCurrentSpawnCount();
        int realCount = Randomizer.GetInRange(minCount, maxCount);
        for (int i = 0; i < realCount; i++)
        {
            if(GameController.GetInstance().GetPassengersCount() > MaxPassengers)
                return;
            int randomIndex = Randomizer.GetInRange(0, unitPrefabs.Count);
            GameObject randomNPC = unitPrefabs[randomIndex];
            float horisontalOffset = Random.Range(-2f, 2f);
            float verticalOffset = Random.Range(1f, 2f);
            Vector3 spawnPosition = new Vector3(spawnPoint.transform.position.x + horisontalOffset, spawnPoint.transform.position.y - verticalOffset, 0);
            GameObject instantiated = (GameObject)Instantiate(randomNPC, spawnPosition, spawnPoint.transform.rotation);
            Passenger ps = instantiated.GetComponent<Passenger>();
            ps.Init();
        }
    }
}
