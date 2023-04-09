using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Range(0, 1)][SerializeField] float m_Volume = 0.1f;
    [SerializeField] Disks[] m_Disks;
    float m_TrackLength;
    int m_CurrentDiskPlaying;

    //Dialogues
    string[] m_Dialogues = { };
    [SerializeField] TextDialogues m_TextDialogue;

    [SerializeField] AudioSource m_Audio;

    // Start is called before the first frame update
    void Start()
    {
        //m_TextDialogue = GameObject.Find("XR Origin").GetComponent<TextDialogues>();
        //m_Audio = gameObject.AddComponent<AudioSource>();
    }
    public float GetVolume()
    {
        return m_Volume;
    }
    public void SetVolume(float newVolume)
    {
        m_Volume = newVolume;
    }
public IEnumerator SoundPlayerOn(string name, float delay)
    {
        for (int i = 0; i < m_Disks.Length; i++)
        {
            if (m_Disks[i].m_Name != name) continue;
            else
            {
                for (int j = 0; j < m_Disks[i].m_Tracks.Length; j++)
                {
                    m_TrackLength = m_Disks[i].m_Tracks[j].length;
                    m_CurrentDiskPlaying = i;
                    m_Audio.clip = m_Disks[i].m_Tracks[j];
                    m_Audio.PlayOneShot(m_Disks[i].m_Tracks[j], m_Volume);
                    if(m_Dialogues.Length != 0)
                        m_TextDialogue.SetText(m_Dialogues[j]);

                    //Debug.Log(m_Dialogues[j]);
                    yield return new WaitForSeconds(m_Disks[i].m_Tracks[j].length + delay);
                    m_Audio.Stop();
                    //m_TextDialogue.SetText("");
                }
                yield break;
            }
        }
    }

    public float GetTrackLength()
    {
        return m_TrackLength;
    }

    public float GetSumTrackLength()
    {
        m_TrackLength = 0.0f;
        for (int i = 0; i < m_Disks[m_CurrentDiskPlaying].m_Tracks.Length; i++)
            m_TrackLength += m_Disks[m_CurrentDiskPlaying].m_Tracks[i].length;
        return m_TrackLength;
    }

    public void StopAnyAudioPlaying()
    {
        m_Audio.Stop();
    }

    public Disks[] GetDisks()
    {
        return m_Disks;
    }

    public void SetDialogues(string[] dialogues)
    {
        m_Dialogues = dialogues;
    }

    public void PlayDisk(string name, float delay)
    {
        StartCoroutine(SoundPlayerOn(name, delay));
    }

    [System.Serializable]
    public class Disks
    {
        public string m_Name;
        public AudioClip[] m_Tracks;
    }
}
