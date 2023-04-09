using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission1Audio : MonoBehaviour
{
    [SerializeField] SoundPlayer m_SoundPlayer;
    [SerializeField] MissionObject m_IceLocatorTrigger;
    [SerializeField] MissionObject m_LavaTubeTrigger;

    bool welcomeMoon = false;
    bool headToEarthRise = false;
    bool iceLocTutorial = false;
    bool headToLaveTube = false;

    bool playEarthRise = false;
    bool playedIce = false;

    float timer = 0;
    bool delayTriggered = true;

    string[] dialogueList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (delayTriggered)
        {
            timer += Time.deltaTime;

            if (timer > 3)
            {
                delayTriggered = false;
            }
        }
        else
        {
            StartCoroutine(WelcomeToMoon());
        }

        if(playEarthRise && !headToEarthRise)
        {
            StartCoroutine(EarthRise());
        }
        else if(m_IceLocatorTrigger.isTriggered && !playedIce)
        {
            StartCoroutine(IceLocTutorial());
        }
        else if (m_LavaTubeTrigger.isComplete)
        {
            StartCoroutine(HeadToLavaTube());
        }
    }

    private IEnumerator WelcomeToMoon()
    {
        if (!welcomeMoon)
        {
            dialogueList = new string[1];
            dialogueList[0] = "Welcome to the moon.";
            m_SoundPlayer.SetDialogues(dialogueList);
            m_SoundPlayer.PlayDisk("WelcomeToMoon", 0);
            welcomeMoon = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());

        playEarthRise = true;
    }

    public IEnumerator EarthRise()
    {
        if (!headToEarthRise)
        {
            dialogueList = new string[1];
            dialogueList[0] = "We will have you head to the top of the ridge to get a good view of the earth rise.";
            m_SoundPlayer.SetDialogues(dialogueList);
            m_SoundPlayer.PlayDisk("HeadToEarthRise", 0);
            headToEarthRise = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
    }

    public IEnumerator IceLocTutorial()
    {
        if (!iceLocTutorial)
        {
            dialogueList = new string[7];
            dialogueList[0] = "Now you will notice a tool attached to your left side, this is an ice locator.";
            dialogueList[1] = "We need you to mark the spot for the robot to drill for some ice.";
            dialogueList[2] = "The color on the screen will tell you how good your aim is.";
            dialogueList[3] = "Red means you are not pointed in the right direction. Yellow means you are getting close.";
            dialogueList[4] = "Green means you have the right direction. Blue means you are aiming right at it.";
            dialogueList[5] = "Once you have found the right direction the slider will represent how close you are to the ice.";
            dialogueList[6] = "When you are confident you have found the source, just pull the trigger and the robot will start drilling.";
            m_SoundPlayer.SetDialogues(dialogueList);
            m_SoundPlayer.PlayDisk("IceLocTutorial", 0);
            iceLocTutorial = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());

        playedIce = true;
    }

    public IEnumerator HeadToLavaTube()
    {
        if (!headToLaveTube)
        {
            dialogueList = new string[1];
            dialogueList[0] = "Excellent, now we will have you save one of the exploration robots, the Asagumo, who has gotten damaged. Head to the marker.";
            m_SoundPlayer.SetDialogues(dialogueList);
            m_SoundPlayer.PlayDisk("HeadToLaveTube", 0);
            headToLaveTube = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
    }
}
