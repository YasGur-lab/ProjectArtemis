using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Controller_GoingToElevator_Scenario : MonoBehaviour
{
    AI_Controller m_AI_Controller;
    [SerializeField] AI_Path m_ElevatorPath;
    GameObject m_Player;

    //SoundPlayer
    SoundPlayer m_SoundPlayer;

    //ControllerTutorial
    ControllerTutorial m_ControllerTutorial;

    //Dialogues
    string[] m_Dialogues;
    bool m_DialogueDone = false;
    //bool m_ClearDialogueDone = false;
    bool m_FunFactDialogueDone = false;
    [SerializeField] bool m_MainNPC = false;

    [SerializeField] TextMeshProUGUI m_SubsTextMesh;

    //Path 
    bool m_ChangingPathDone = false;
    [SerializeField] private Transform m_Destination;

    //Spacesuit
    [SerializeField] GameObject m_Spacesuit;

    //Elevator panel indicator
    [SerializeField] GameObject m_Panel;
    bool m_ChangingPathToGoInsideElevatorDone = false;
    [SerializeField] AI_Path m_ToGoInElevatorPath;

    //Doors
    [SerializeField] private DoorOpener[] m_Doors;

    // Start is called before the first frame update
    void Start()
    {
        m_AI_Controller = GetComponent<AI_Controller>();
        m_Player = GameObject.Find("XR Origin");
        m_SoundPlayer = GetComponent<SoundPlayer>();
        m_ControllerTutorial = GetComponent<ControllerTutorial>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HasSuitOn())
        {
            if (m_Spacesuit)
            {
                foreach (var door in m_Doors)
                {
                    door.SetReadyToBeOpen(true);
                }

                m_Spacesuit.GetComponent<Highlight>().IsActive(false);
            }

            if(m_Panel)
                m_Panel.GetComponent<Highlight>().IsActive(true);

            if (m_MainNPC)
                StartCoroutine(GoingToElevator());
            else
            {
                if (!m_ChangingPathDone)
                {
                    //m_AI_Controller.RemovePath();
                    //m_AI_Controller.ChangePath(m_ElevatorPath);
                    m_ChangingPathDone = true;
                }
            }
        }
        else
        {
            if (m_ControllerTutorial.HasFinishedFinalDialogue())
            {
                if (m_Spacesuit)
                {
                    m_Spacesuit.GetComponent<Highlight>().IsActive(true);
                }
            }
        }
        
    }

    IEnumerator GoingToElevator()
    {
        if (!m_DialogueDone)
        {
            m_Dialogues = new string[1];
            m_Dialogues[0] = "Perfect, now you can head for the elevator and make your way to the Canoo SUV, who will take you to the launch site."; //needs to be fixed
            m_SoundPlayer.SetDialogues(m_Dialogues);
            m_SoundPlayer.PlayDisk("GoingToElevator_SuitOn", 0);
            m_DialogueDone = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());

        m_SubsTextMesh.text = "";


        if (!m_ChangingPathDone)
        {
            m_AI_Controller.SetDestination(m_Destination);
            //m_AI_Controller.ChangePath(m_ElevatorPath);
            m_ChangingPathDone = true;

        }
        yield return new WaitForSeconds(7.0f);

        //m_HasFinishedDialogue = false;
        if (!m_FunFactDialogueDone)
        {
            /*m_Dialogues = new string[2];
            m_Dialogues[0] = "If I were you I'd be excited about the food most. They have freeze-dried ice-cream in the gift shop, I can't get enough of that stuff. The wife says it's too much sugar for an old guy like me, but don't tell her I grab some everyday."; //needs to be fixed
            m_Dialogues[1] = "Hey kid, don't look so nervous! You're not the first to launch from here. They've been shooting rockets to space with astronauts in em' since 1968."; //needs to be fixed
            m_SoundPlayer.SetDialogues(m_Dialogues);
            m_SoundPlayer.PlayDisk("GoingToElevator_FunFacts", 7);*/
            m_FunFactDialogueDone = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
    }

    public bool HasSuitOn()
    {
        return m_Player.GetComponentInChildren<Equipment>().IsSpacesuitOn();
    }

    public void SwitchPath()
    {
        if (!m_ChangingPathToGoInsideElevatorDone)
        {
            //m_AI_Controller.RemovePath();
            //m_AI_Controller.ChangePath(m_ToGoInElevatorPath);
            m_ChangingPathToGoInsideElevatorDone = true;
        }
    }
}
