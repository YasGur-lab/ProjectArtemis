using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: Scenario1_Elevator_Sequence
Author: Antoine Plouffe
Created: 01/09/2023
Last Modified: 02/12/2022
Description: This class is in charge of the Elevator Sequence in scenario 1.

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

public class Scenario1_Elevator_Sequence : MonoBehaviour
{
    //AI
    [SerializeField] GameObject m_AI;

    //Elevator
    [SerializeField] Elevator m_Elevator;

    //Stages
    ElevatorStages m_Stages;

    //SoundPlayer
    string[] m_Dialogues;
    SoundPlayer m_SoundPlayer;
    private bool m_ElevatorDialogueDone = false;

    //AI
    [SerializeField] AI_Controller[] m_AI_Controller;
    bool m_ChangingPathToGoInsideElevatorDone = false;
    [SerializeField] AI_Path[] m_ToGoOutElevatorPath;

    // Start is called before the first frame update
    void Start()
    {
        m_SoundPlayer = m_AI.GetComponent<SoundPlayer>();
        m_Stages = ElevatorStages.NotActive;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Stages == ElevatorStages.Inside)
            StartCoroutine(ElevatorSequence());
    }

    private IEnumerator ElevatorSequence()
    {
        if (!m_ElevatorDialogueDone)
        {
            m_Dialogues = new string[2];
            m_Dialogues[0] = "You know, only 9 percent of the KSC land is developed. They sure love their space. Ha! Get it?";
            m_Dialogues[1] = "Did you know most of the undeveloped land you see is Wildlife Sanctuary, they gotta be good to the planet they leave behind.";
            m_SoundPlayer.SetDialogues(m_Dialogues);
            m_SoundPlayer.PlayDisk("InsideElevator_FunFacts", 1);
            m_ElevatorDialogueDone = true;
        }

        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
    }

    public void SwitchPath()
    {
        if (!m_ChangingPathToGoInsideElevatorDone)
        {

            for (int i = 0; i < m_AI_Controller.Length; i++)
            {
                //m_AI_Controller[i].RemovePath();
                //m_AI_Controller[i].ChangePath(m_ToGoOutElevatorPath[i]);
            }
            m_ChangingPathToGoInsideElevatorDone = true;
        }
    }

    enum ElevatorStages
    {
        NotActive,
        GoingTo,
        Inside,
        Out,
        Done,
    }

    public void SetStageToInside()
    {
        m_Stages = ElevatorStages.Inside;
    }
    public void SetStageToOut()
    {
        m_Stages = ElevatorStages.Inside;
    }
}
