using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

/*************************************
Class: Screenshots
Author: Antoine Plouffe
Created: 01/09/2022
Last Modified: 02/12/2022
Description: This class is in charge of handling screenshots.

Change Log
**********
Date: 02/12/2022
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/


public class Screenshots : MonoBehaviour
{
    private Camera m_MediaCamera;
    private string m_Path = "";
    [SerializeField] bool m_TakePicture = false;
    [SerializeField] GameObject m_Plane;
    [SerializeField][Range(0.0f, 10.0f)] float m_TimeBeforeScreenshot = 0.0f;

    void Start()
    {
        m_MediaCamera = GameObject.Find("Media_Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if(!m_TakePicture)
            m_MediaCamera.enabled = false;
    }

    public IEnumerator TakeScreenShot()
    {
        yield return new WaitForSeconds(m_TimeBeforeScreenshot);
        yield return new WaitForEndOfFrame();
        m_MediaCamera.enabled = true;
        m_TakePicture = false;

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = m_MediaCamera.targetTexture;
        m_MediaCamera.Render();
        Texture2D screenshotTexture = new Texture2D(m_MediaCamera.targetTexture.width, m_MediaCamera.targetTexture.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, m_MediaCamera.targetTexture.width, m_MediaCamera.targetTexture.height), 0, 0);
        screenshotTexture.Apply();
        m_Plane.GetComponent<MeshRenderer>().material.mainTexture = screenshotTexture;
        RenderTexture.active = currentRT;

        // Encode texture into PNG
        byte[] bytes = screenshotTexture.EncodeToPNG();

        // Increment file name
        int filenumber = 0;
        int length = 300;
        string filename = "Screenshot" + filenumber.ToString();
        m_Path = Path.Combine(Application.persistentDataPath, filename + ".png");

        for (int i = 0; i < length; i++)
        {
            if (!System.IO.File.Exists(m_Path))
            {
                //Save screenshot in memory
                System.IO.File.WriteAllBytes(m_Path, bytes);
                Debug.Log(m_Path);
                yield break;
            }
            filenumber++;
            filename = "Screenshot" + filenumber.ToString();
            m_Path = Path.Combine(Application.persistentDataPath, filename + ".png");
        }
    }

    public void TakePicture()
    {
        m_TakePicture = true;
    }

    public bool IsTakingPicture()
    {
        return m_TakePicture;
    }
}
