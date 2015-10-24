using System.Collections.Generic;
using Assets;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> unitPrefabs;
    private float _maxPassengers;

    public static float StickYOffset = 0.8f;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();

        _maxPassengers = ConfigReader.GetConfig().GetField("tram").GetField("MaxPassengers").n;
        GameController.GetInstance().StartNewGame();
    }

    public void Spawn(GameObject spawnPoint)
    {
        if(GameController.GetInstance().IsGameFinished)
            return;
        int maxCount = GameController.GetInstance().GetCurrentSpawnCount();
        int realCount = 1;
        if(GameController.GetInstance().GetCurrentStationNumber() > 0)
            realCount = Randomizer.GetInRange(1, maxCount);
        
        for (int i = 0; i < realCount; i++)
        {
            if(GameController.GetInstance().GetPassengersCount() > _maxPassengers)
                return;
            int randomIndex = Randomizer.GetInRange(0, unitPrefabs.Count);
            GameObject randomNPC = unitPrefabs[randomIndex];
            Vector3 spawnPosition = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y);
            GameObject instantiated =
                        (GameObject)Instantiate(randomNPC, spawnPosition, spawnPoint.transform.rotation);
            PassengerSM ps = instantiated.GetComponent<PassengerSM>();
            ps.Init();
            MonobehaviorHandler.GetMonobeharior().GetObject<BonusTimer>("bonusTimer").AddBonusEffectToSpawnedPassenger(ps);
            if (ps.IsStick())
            {
                DoorsTimer timer = GetComponent<DoorsTimer>();
                timer.SetPaused(true);
                return;
            }
            ps.CalculateRandomTarget();
        } 
    }
}
