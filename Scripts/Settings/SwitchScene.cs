using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] bool m_TestingEnvironmentOn = false;
    [SerializeField] bool m_ScenarioOneOn = false;

    private void Update()
    {
        if (m_TestingEnvironmentOn)
        {
            LoadTestingEnvironment();
            m_TestingEnvironmentOn = false;
            m_ScenarioOneOn =false;
        }

        if (m_ScenarioOneOn)
        {
            LoadScenarioOne();
            m_TestingEnvironmentOn = false;
            m_ScenarioOneOn = false;
        }
    }

    public void LoadTestingEnvironment()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void LoadScenarioOne()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
