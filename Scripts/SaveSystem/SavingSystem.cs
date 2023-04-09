using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/*************************************
Class: SavingSystem
Author: Antoine Plouffe
Created: 01/09/2023
Last Modified: 02/12/2022
Description: This class is in charge of the saving system.

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

public class SavingSystem : MonoBehaviour
{
    public IEnumerator LoadLastScene(string saveFile)
    {
        Dictionary<string, object> state = LoadFile(saveFile);

        //#if UNITY_EDITOR
        int buildIndex = 0;
        yield return SceneManager.LoadSceneAsync(buildIndex);
        //#endif
        //int buildIndex = SceneManager.GetActiveScene().buildIndex;
        //if (state.ContainsKey("lastSceneBuildIndex"))
        //    buildIndex = (int)state["lastSceneBuildIndex"];
        yield return SceneManager.LoadSceneAsync(buildIndex);
        LoadState(state);
    }

    public void Save(string saveFile)
    {
        Dictionary<string, object> state = LoadFile(saveFile);
        SaveState(state);
        SaveFile(saveFile, state);
    }

    public void Load(string saveFile)
    {
        LoadState(LoadFile(saveFile));
    }

    public void Delete(string saveFile)
    {
        File.Delete(GetPathFromSaveFile(saveFile));
    }

    private Dictionary<string, object> LoadFile(string saveFile)
    {
        string path = GetPathFromSaveFile(saveFile);
        if (!File.Exists(path))
        {
            Debug.Log("No save file found.");
            return new Dictionary<string, object>();
        }
        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void SaveFile(string saveFile, object state)
    {
        string path = GetPathFromSaveFile(saveFile);
        print("Saving to " + path);
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private void SaveState(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            state[saveable.GetUniqueIdentifier()] = saveable.SaveState();
        state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
    }

    private void LoadState(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            string id = saveable.GetUniqueIdentifier();
            if (state.ContainsKey(id))
                saveable.LoadState(state[id]);
        }
    }

    private string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}
