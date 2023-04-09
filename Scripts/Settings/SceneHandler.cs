using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

/*************************************
Class: SceneHandler
Author: Antoine Plouffe
Created: 01/27/23
Last Modified: 01/27/23
Description: This class is in charge of Scene Handling.

Change Log
**********
Date: 01/27/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created

Date: 02/02/23
Author: Antoine Plouffe
Verified By: Alexander Achorn 
Changes: Changes ScenesID names to reflect Scene names

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Removed excessive save that was invalidating all save date by saving the second a scene loaded and then loading the save it just overwrote
*************************************/

public class SceneHandler : MissionObject
{
    [SerializeField] private int m_SceneToLoad = -1;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] SceneID m_SceneID;
    [SerializeField] SceneToGoTo m_SceneToGoTo;

    enum SceneID
    {
        MainMenu, Scenario1, Scenario2, Scenario3, MoonLanding, LavaTube, Surveyor5, ReturnToShip, TestingEnvironment
    }

    enum SceneToGoTo
    {
        MainMenu, Scenario1, Scenario2, Scenario3, MoonLanding, LavaTube, Surveyor5, ReturnToShip, TestingEnvironment
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        if (m_SceneToLoad < 0)
        {
            Debug.LogError("Scene to load not set.");
            yield break;
        }

        DontDestroyOnLoad(gameObject);

        Fade fader = FindObjectOfType<Fade>();

        fader.FadeOut();
        //yield return new WaitForSeconds(fader.GetFadeDuration());

        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.Save();
        yield return SceneManager.LoadSceneAsync(m_SceneToLoad);
        wrapper.Load();

        SceneHandler otherScene = GetOtherScene();
        UpdatePlayer(otherScene);

        yield return new WaitForSeconds(fader.GetFadeDuration());
        fader.FadeIn();

        Destroy(gameObject, 1f);
    }

    private SceneHandler GetOtherScene()
    {
        foreach (SceneHandler scene in FindObjectsOfType<SceneHandler>())
        {
            if (scene == this) continue;
            if (scene.m_SceneID == (SceneID)m_SceneToGoTo)
                return scene;
        }
        return null;
    }

    private void UpdatePlayer(SceneHandler otherScene)
    {
        GameObject player = GameObject.FindWithTag("Player");

        player.transform.position = otherScene.m_SpawnPoint.position;
        player.transform.rotation = otherScene.m_SpawnPoint.rotation;

        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.Load();
    }
}
