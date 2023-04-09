using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission2Audio : MonoBehaviour
{
    [SerializeField] SoundPlayer m_SoundPlayer;
    public bool m_wiresExposed = false;
    public bool m_removeBat = false;
    public bool m_fixed = false;

    [SerializeField] MissionObject climbingTrigger;

    bool thisIsLavaTube = false;
    bool wiresExposed = false;
    bool removeBattery = false;
    bool fixedAsago = false;
    bool toClimb = false;

    bool allFixed = false;

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

            if (timer > 1)
            {
                delayTriggered = false;
            }
        }
        else
        {
            StartCoroutine(ThisIsLavaTube());
        }

        if(m_wiresExposed && !m_removeBat)
        {
            StartCoroutine(WiresExposed());
        }

        if(m_removeBat && !m_fixed)
        {
            StartCoroutine(RemoveBattery());
        }
        
        if (m_fixed && !allFixed)
        {
            StartCoroutine(FixedAsago());
        }

        if (allFixed && climbingTrigger.isComplete)
        {
            StartCoroutine(ToClimb());
        }
    }

    private IEnumerator ThisIsLavaTube()
    {
        if (!thisIsLavaTube)
        {
            dialogueList = new string[3];
            dialogueList[0] = "This is a lava tube and the Asagumo has broken down halfway through.";
            dialogueList[1] = "Using the drill to your left side we need you to unscrew the panel and swap out the power source on the Asagumo.";
            dialogueList[2] = "You also have a flare gun on your right side to use to mark your way and to provide some additional light as you make your way down to find the Asagumo.";
            m_SoundPlayer.SetDialogues(dialogueList);
            m_SoundPlayer.PlayDisk("ThisIsLavaTube", 0);
            thisIsLavaTube = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
    }

    public IEnumerator WiresExposed()
    {
        if (!wiresExposed)
        {
            dialogueList = new string[1];
            dialogueList[0] = "Well done, the wires are now exposed. To remove the power source, grab each wire and remove them one by one.";
            m_SoundPlayer.SetDialogues(dialogueList);
            m_SoundPlayer.PlayDisk("WiresExposed", 0);
            wiresExposed = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
    }

    public IEnumerator RemoveBattery()
    {
        if (!removeBattery)
        {
            dialogueList = new string[1];
            dialogueList[0] = "Excellent work, remove the faulty power source and replace it with the new one. ";
            m_SoundPlayer.SetDialogues(dialogueList);
            m_SoundPlayer.PlayDisk("RemoveBattery", 0);
            removeBattery = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
    }

    public IEnumerator FixedAsago()
    {
        if (!fixedAsago)
        {
            dialogueList = new string[1];
            dialogueList[0] = "Perfect, you have successfully repaired the Asagumo.";
            m_SoundPlayer.SetDialogues(dialogueList);
            m_SoundPlayer.PlayDisk("FixedAsago", 0);
            fixedAsago = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());

        allFixed = true;
        climbingTrigger.gameObject.SetActive(true);
    }
    public IEnumerator ToClimb()
    {
        if (!toClimb)
        {
            dialogueList = new string[3];
            dialogueList[0] = "To climb, put your hand into a climbing rock and grab using the button by your pinky and ring finger.";
            dialogueList[1] = "While holding a rock pull downwards to lift yourself up.";
            dialogueList[2] = "Now you can reach the next rock and continue until you have reached the top.";
            m_SoundPlayer.SetDialogues(dialogueList);
            m_SoundPlayer.PlayDisk("ToClimb", 0);
            toClimb = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
    }
}
