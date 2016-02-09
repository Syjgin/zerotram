using UnityEngine;
using System.Collections;

public class SpawnerProps : MonoBehaviour
{

    public float spawnTime = 2f;//Промежуток времени между спавном, он же определяет и "скорость" движения трамвая
    public float spawnDelay = 1f;//Кол-во объектов,кот. спавнятся
    public GameObject smt;//То, что спавним

    void Start()
    {
        InvokeRepeating("Spawn", spawnDelay, spawnTime);
    }

    void Spawn()
    {
        Instantiate(smt, transform.position, transform.rotation);
    }
}
