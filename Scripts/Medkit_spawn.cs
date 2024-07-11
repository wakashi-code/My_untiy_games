using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit_spawn : MonoBehaviour
{
    public GameObject Little_medkit;
    public Transform[] spawnPoints;
    public float spawnInterval = 5;
    public bool medkit_spawned = false;
    
    void Update()
    {


        if (spawnInterval > 0)
        {
            spawnInterval -= Time.deltaTime;
            if (spawnInterval <= 0 && !medkit_spawned)
            {
                Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
                GameObject med = Instantiate(Little_medkit, randomPoint.position, Quaternion.identity);
                medkit_spawned = true;
            }
        }
        else
        {
            spawnInterval = 5;
        }
    }
}
