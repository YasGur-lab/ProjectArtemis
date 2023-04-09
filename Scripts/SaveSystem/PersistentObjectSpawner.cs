using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: PersistentObjectSpawner
Author: Antoine Plouffe
Created: 01/09/2023
Last Modified: 02/12/2022
Description: This class is in charge of managing all persistent gameObjects and spawns them.

Change Log
**********
Date: 02/09/2022
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_PersistentObjectPrefab;
    private static bool m_hasSpawned = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (m_hasSpawned) return;
        SpawnPersistentObjects();

        m_hasSpawned = true;
    }

    private void SpawnPersistentObjects()
    {
        GameObject persistentObjects = Instantiate(m_PersistentObjectPrefab);
        DontDestroyOnLoad(persistentObjects);
    }
}
