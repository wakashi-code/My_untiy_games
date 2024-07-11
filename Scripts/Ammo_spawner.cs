using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_spawner : MonoBehaviour
{

    public GameObject Ammo_box;
    public Transform[] spawnPoints;
    public float interval = 5;

    public bool ammo_box_spawned = false;
    
  

    void Update()
    {
        if (interval > 0 && !ammo_box_spawned )
        {
            interval -= Time.deltaTime;
            if (interval <= 0)
            {
                Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

                GameObject box = Instantiate(Ammo_box, randomPoint.position, Quaternion.identity);
                ammo_box_spawned = true;
  
            }
        }
        else
        {
            interval = 5;
        }
    }
}
