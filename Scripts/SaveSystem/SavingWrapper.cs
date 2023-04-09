using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: SavingWrapper
Author: Antoine Plouffe
Created: 01/09/2023
Last Modified: 02/12/2022
Description: This class is in charge of wrapping the saving system.

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

public class SavingWrapper : MonoBehaviour
{
    private const string m_DefaultSaveFile = "save";

    private void Awake()
    {
        StartCoroutine(LoadLastScene());
    }

    IEnumerator LoadLastScene()
    {
        yield return GetComponent<SavingSystem>().LoadLastScene(m_DefaultSaveFile);
    }

    public void Save()
    {
        //Debug.Log("Saving Savefile...");
        GetComponent<SavingSystem>().Save(m_DefaultSaveFile);
    }

    public void Load()
    {
        //Debug.Log("Loading Savefile...");
        GetComponent<SavingSystem>().Load(m_DefaultSaveFile);
    }
}
