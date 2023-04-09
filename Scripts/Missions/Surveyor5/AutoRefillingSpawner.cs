using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: AutoRefillingSpawner
Author: Charlie Weir
Created: 06/03/2022
Last Modified: 06/03/2022
Description: This class will spawn objects into another object.

Change Log
**********
Date: 06/03/2022
Author: Charlie Weir
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class AutoRefillingSpawner : MonoBehaviour
{

    public GameObject ObjectInsidePrefab;
    public int NumObjectsInside = 7;
    public float RespawnDist = 200.0f;
    float TimeSinceLastSpawn = 0.0f;
    float TimeBetweenSpawns = 2.0f;
    int NumToSpawn = 0;
    List<GameObject> gameObjects;
    public GameObject spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        NumToSpawn = NumObjectsInside;
        gameObjects = new List<GameObject>();
    }
    void ReplaceObject(int RemovedIndex)
    {
        gameObjects[RemovedIndex].transform.position = spawnPoint.transform.position;
        gameObjects[RemovedIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    GameObject SpawnObject()
    {
        GameObject NewObject = GameObject.Instantiate(ObjectInsidePrefab);
        NewObject.transform.position = spawnPoint.transform.position;
        gameObjects.Add(NewObject);
        return NewObject;

    }
    // Update is called once per frame
    void Update()
    {
        if (NumToSpawn > 0)
        {
            TimeSinceLastSpawn += Time.deltaTime;
            if (TimeSinceLastSpawn > TimeBetweenSpawns)
            {
                TimeSinceLastSpawn = 0;
                NumToSpawn--;
                SpawnObject();
            }
        }

        for (int i = 0; i < gameObjects.Count; i++)
        {
            float Sqrdist = Vector3.SqrMagnitude(gameObjects[i].transform.position - transform.position);
            if (Sqrdist > RespawnDist * RespawnDist)
            {
                ReplaceObject(i);
            }
        }
    }
}

