using System.Collections.Generic;
using Assets;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> unitPrefabs;

    private const float Offset = 1;

    public void Spawn(GameObject spawnPoint)
    {
        int maxCount = GameController.GetInstance().GetCurrentSpawnCount();
        int realCount = Randomizer.GetInRange(0, maxCount);
        for (int i = 0; i < realCount; i++)
        {
            int randomIndex = Randomizer.GetInRange(0, unitPrefabs.Count);
            GameObject randomNPC = unitPrefabs[randomIndex];
            Vector3 spawnPosition = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y - Offset, 0);
            GameObject instantiated = (GameObject)Instantiate(randomNPC, spawnPosition, spawnPoint.transform.rotation);
            Passenger ps = instantiated.GetComponent<Passenger>();
            ps.Init();
        }
    }
}
