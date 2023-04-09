using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*************************************
Class: SaveableEntity
Author: Antoine Plouffe
Created: 01/09/2023
Last Modified: 02/12/2022
Description: This class is in charge of the saving entities into the saving system.

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

[ExecuteAlways]
public class SaveableEntity : MonoBehaviour
{
    [SerializeField] string m_UniqueIdentifier = "";
    static Dictionary<string, SaveableEntity> m_GlobalLookup = new Dictionary<string, SaveableEntity>();

    public string GetUniqueIdentifier()
    {
        return m_UniqueIdentifier;
    }

    public object SaveState()
    {
        Dictionary<string, object> saveStateDict = new Dictionary<string, object>();

        foreach (ISaveableList saveableList in GetComponents<ISaveableList>())
        {
            saveStateDict[saveableList.SaveStates().Count.GetType().ToString()] = saveableList.SaveStates().Count;
            foreach (bool state in saveableList.SaveStates())
            {
                saveStateDict[state.GetType().ToString()] = state;
            }
        }

        foreach (ISaveable saveable in GetComponents<ISaveable>())
            saveStateDict[saveable.GetType().ToString()] = saveable.SaveState();

        return saveStateDict;
    }

    public void LoadState(object state)
    {
        Dictionary<string, object> loadStateDict = (Dictionary<string, object>)state;

        foreach (ISaveableList saveableList in GetComponents<ISaveableList>())
        {
            List<bool> listStates = new List<bool>();
            int listSize = 0;

            string typeName = listStates.Count.GetType().ToString();
            if (loadStateDict.ContainsKey(typeName))
                listSize = (int)loadStateDict[typeName];

            for (int i = 0; i < listSize; i++)
            {
                bool empty = false;
                if (loadStateDict.ContainsKey(empty.GetType().ToString()))
                    listStates.Add((bool)loadStateDict[typeName]);

            }

            saveableList.LoadStates(listStates);
        }

        foreach (ISaveable saveable in GetComponents<ISaveable>())
        {
            string typeName = saveable.GetType().ToString();
            if (loadStateDict.ContainsKey(typeName))
                saveable.LoadState(loadStateDict[typeName]);
        }
    }
}