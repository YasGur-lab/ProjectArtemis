using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

/*************************************
Class: ControllerTutorial
Author: Antoine Plouffe
Created: 01/09/22
Last Modified: 17/01/23
Description: This class is in charge of teaching the player about their controllers. They will learn how to move, teleport and grab objects.

Change Log
**********
Date: 17/01/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Optimizing the script to be less ressource intensive.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class ControllerTutorial : MonoBehaviour
{
    //AI_Controller m_AI_Controller;

    [SerializeField] GameObject m_Player;
    [SerializeField] bool m_TutorialIsTrigged = false;
    [SerializeField] bool m_TutorialHasBegan = false;
    [SerializeField][Range(0.0f, 10.0f)] float m_TimerBeforeTutorial = 5.0f;

    [SerializeField] Highlight[] m_HighlightsList;

    //Controllers
    [SerializeField] XRDirectInteractor[] m_Interactors;
    List<UnityEngine.XR.InputDevice> m_LeftController;
    List<UnityEngine.XR.InputDevice> m_RightController;
    [SerializeField][Range(0.0f, 1.0f)] float m_HapticStrength = 1.0f;
    [SerializeField][Range(0.0f, 1.0f)] float m_HapticDuration = 1.0f;

    TutorialStages m_Stage;

    //Renderers
    [SerializeField] Renderer[] m_LeftController_RenderersList;
    [SerializeField] Renderer[] m_RightController_RenderersList;
    bool m_AddRenderers = false;

    //Teleportation Tutorial
    int m_NumOfTeleportations = 0;
    [SerializeField][Range(0, 10)] int m_NumOfTeleportationsObjective = 3;
    [SerializeField] InputActionReference m_Input;
    //ActionBasedMovement Tutorial
    float m_NumOfTimeInMouvement = 0.0f;
    [SerializeField][Range(0.0f, 10.0f)] float m_NumOfTimeInMouvementObjective = 3.0f;
    bool m_IsMoving = false;

    //Grab Tutorial
    int m_NumOfGrabs = 0;
    [SerializeField][Range(0, 10)] int m_NumOfGrabObjective = 3;
    bool m_IsGrabbing = false;
    [SerializeField] private XRSocketInteractor m_TabletAnchor;

    //SoundPlayer
    SoundPlayer m_SoundPlayer;
    bool m_HasFinishedDialogue = false;

    //Dialogues
    string[] m_Dialogues;
    bool m_MovementDialogueDone = false;
    bool m_TeleportDialogueDone = false;
    bool m_GrabDialogueDone = false;
    bool m_GrabDialoguePart2Done = false;
    bool m_GrabDialoguePart3Done = false;
    bool m_FinalDialogueDone = false;
    bool m_ResetDialogueDone = false;


    //Haptics
    bool m_MovementHapticDone = false;
    bool m_TeleportHapticDone = false;
    bool m_GrabHapticDone = false;

    //Input
    HandlingInputs m_HandlingInputs;
    private bool m_InputsEnabled = false;

    //Spacesuit
    [SerializeField] GameObject m_Spacesuit;

    //Tablet
    [SerializeField] GameObject m_Tablets;

    //Controllers
    [SerializeField] private GameObject m_LeftGOController;
    [SerializeField] private GameObject m_RightGOController;

    //Doors
    [SerializeField] private DoorOpener[] m_Doors;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.Find("XR Origin");
        m_SoundPlayer = GetComponent<SoundPlayer>();
        m_LeftController = new List<UnityEngine.XR.InputDevice>();
        m_RightController = new List<UnityEngine.XR.InputDevice>();
        m_Stage = TutorialStages.NotActive;
        m_HandlingInputs = m_Player.GetComponent<HandlingInputs>();

        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, m_LeftController);
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, m_RightController);
        m_Tablets.GetComponent<Highlight>().IsActive(false);

        m_Input.action.canceled += TeleportActivated;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Scenario_1")
        {
            if (m_Stage != TutorialStages.Done)
            {
                if (m_LeftController.Count == 0 || m_RightController.Count == 0)
                {
                    UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, m_LeftController);
                    UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, m_RightController);
                }

                if (m_TutorialHasBegan)
                {
                    if (!m_LeftGOController.activeSelf) m_LeftGOController.SetActive(true);
                    if (!m_RightGOController.activeSelf) m_RightGOController.SetActive(true);
                    StartingTutorial();
                }

                if (m_Stage == TutorialStages.BasicMovements)
                {
                    if (!m_MovementHapticDone) StartCoroutine(Tutorial_BasicMovements());
                    else ActionBasedMovements();
                }
                else if (m_Stage == TutorialStages.Teleportation)
                {
                    if (!m_TeleportHapticDone)
                        StartCoroutine(Tutorial_Teleportation());
                    else
                        TeleportationBasedMovements();
                }
                else if (m_Stage == TutorialStages.Grab)
                {
                    if (!m_GrabHapticDone) StartCoroutine(Tutorial_Grab());
                    else StartCoroutine(GrabbingObject());
                }

                /*foreach (var highlight in m_HighlightsList)
                {
                    if (m_HighlightsList.Length < 1) return;
                    highlight.IsActive(true);
                }*/
            }
            else
            {
                if (m_LeftGOController.activeSelf) m_LeftGOController.SetActive(false);
                if (m_RightGOController.activeSelf) m_RightGOController.SetActive(false);
            }
        }
    }

    private IEnumerator Tutorial_BasicMovements()
    {
        //if (m_AddRenderers)
        //{
        //    Renderer[] tempRend = new Renderer[1];
        //    tempRend[0] = m_LeftController_RenderersList[0];
        //    AddRenderer(tempRend, m_HighlightsList[0]);
        //}

        if (!m_MovementDialogueDone)
        {
            GetComponent<AI_Controller>().SetAIToMove(false);

            m_Dialogues = new string[3];
            m_Dialogues[0] = "Well hello, command told me to reach out to give you a rundown of the controls since this will be your first time in space.";
            m_Dialogues[1] = "To move use the left controllers stick or touchpad to walk.";
            m_Dialogues[2] = "To rotate, do so in real life or use the stick or touchpad on your right controller.";
           m_SoundPlayer.SetDialogues(m_Dialogues);
            m_SoundPlayer.PlayDisk("TutorialMovement", 0);
            m_ResetDialogueDone = false;
            m_MovementDialogueDone = true;
        } 

        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
        
        if (!m_MovementHapticDone)
        {
            m_Interactors[0].SendHapticImpulse(m_HapticStrength, m_HapticDuration);
            m_MovementHapticDone = true;
        }

        

        m_HasFinishedDialogue = false;
        m_InputsEnabled = false;

        if (!m_HasFinishedDialogue && m_MovementDialogueDone)
        {
            GetComponent<AI_Controller>().SetAIToMove(true);
            m_HasFinishedDialogue = true;
        }

        if (!m_InputsEnabled)
        {
            m_HandlingInputs.EnableInputs("XRI LeftHand Locomotion", "Turn");
            m_HandlingInputs.EnableInputs("XRI LeftHand Locomotion", "Move");
            m_HandlingInputs.EnableInputs("XRI RightHand Locomotion", "Turn");
            m_HandlingInputs.EnableInputs("XRI RightHand Locomotion", "Move");
            m_InputsEnabled = true;
        }

        //if (!m_MovementHapticDone && m_InputsEnabled)
        //{
        //    m_Interactors[0].SendHapticImpulse(m_HapticStrength, m_HapticDuration);
        //    m_MovementHapticDone = true;
        //}
    }

    private IEnumerator Tutorial_Teleportation()
    {
        if (m_NumOfTeleportations > 0) m_NumOfTeleportations = 0;

        //if (m_AddRenderers)
        //{
        //    Renderer[] tempRend = new Renderer[1];
        //    tempRend[0] = m_RightController_RenderersList[0];
        //    AddRenderer(tempRend, m_HighlightsList[1]);
        //}

        if (!m_TeleportDialogueDone)
        {
            GetComponent<AI_Controller>().SetAIToMove(false);

            m_Dialogues = new string[1];
            m_Dialogues[0] = "If free movement makes you nauseous, you can use the right trigger to teleport. Hold it to aim, then release to teleport. Your left trigger lets you jump.";
            m_SoundPlayer.SetDialogues(m_Dialogues);
            m_SoundPlayer.PlayDisk("TutorialTeleportation", 0);
            m_ResetDialogueDone = false;
            m_TeleportDialogueDone = true;
        }

        if (!m_TeleportHapticDone)
        {
            m_Interactors[1].SendHapticImpulse(m_HapticStrength, m_HapticDuration); ;
            m_TeleportHapticDone = true;
        }

        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());

        m_HasFinishedDialogue = false;
        m_InputsEnabled = false;

        if (!m_HasFinishedDialogue && m_TeleportDialogueDone && m_MovementDialogueDone)
        {
            GetComponent<AI_Controller>().SetAIToMove(true);
            m_HasFinishedDialogue = true;
        }

        if (!m_InputsEnabled)
        {
            m_HandlingInputs.EnableInputs("XRI LeftHand Interaction");
            m_HandlingInputs.EnableInputs("XRI RightHand Interaction");
            m_HandlingInputs.EnableInputs("XRI LeftHand Locomotion");
            m_HandlingInputs.EnableInputs("XRI RightHand Locomotion");
            m_InputsEnabled = true;
        }

        //if (!m_TeleportHapticDone && m_InputsEnabled)
        //{
        //    m_Interactors[1].SendHapticImpulse(m_HapticStrength, m_HapticDuration); ;
        //    m_TeleportHapticDone = true;
        //}
    }

    private IEnumerator Tutorial_Grab()
    {
        //if (m_AddRenderers)
        //{
        //    Renderer[] tempRend = new Renderer[4];
        //    tempRend[0] = m_LeftController_RenderersList[1];
        //    tempRend[1] = m_LeftController_RenderersList[2];
        //    tempRend[2] = m_RightController_RenderersList[1];
        //    tempRend[3] = m_RightController_RenderersList[2];
        //    AddRenderer(tempRend, m_HighlightsList[0]);
        //}

        if (!m_GrabDialogueDone)
        {
            GetComponent<AI_Controller>().SetAIToMove(false);

            /*m_Dialogues = new string[1];
            m_Dialogues[0] = "See… easy! Finally, let’s grab your equipment. Follow me.";
            m_SoundPlayer.SetDialogues(m_Dialogues);
            m_SoundPlayer.PlayDisk("TutorialGrab", 0);*/
            m_ResetDialogueDone = false;
            m_GrabDialogueDone = true;
        }
        //yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());

        if (!m_GrabHapticDone)
        {
            m_Interactors[0].SendHapticImpulse(m_HapticStrength, m_HapticDuration);
            m_Interactors[1].SendHapticImpulse(m_HapticStrength, m_HapticDuration);
            m_GrabHapticDone = true;
        }

        m_HasFinishedDialogue = false;
        m_InputsEnabled = false;

        if (!m_HasFinishedDialogue && m_GrabDialogueDone && m_TeleportDialogueDone && m_MovementDialogueDone)
        {
            GetComponent<AI_Controller>().SetAIToMove(true);
            m_HasFinishedDialogue = true;
        }
        
        //yield return new WaitUntil(() => GetComponent<AI_Controller>().InRangeOfDestination());

        if (!m_GrabDialoguePart2Done)
        {
            foreach (var door in m_Doors)
            {
                //door.SetReadyToBeOpen(false);
                m_Doors[2].Close();
                m_Doors[3].Close();
            }

            m_HasFinishedDialogue = false;

            m_Dialogues = new string[2];
            m_Dialogues[0] = "When you need to pick something up, use the button by your pinky and ring finger to grab and hold. As long as you are holding it, you will continue to hold the object.";
            m_Dialogues[1] = "Grab a tablet and put it on your back.";
            m_SoundPlayer.SetDialogues(m_Dialogues);
            m_SoundPlayer.PlayDisk("TutorialPart2Grab", 0);
            m_GrabDialoguePart2Done = true;

        }

        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());

        if (!m_HasFinishedDialogue) m_HasFinishedDialogue = true;

        if (!m_InputsEnabled)
        {
            m_HandlingInputs.EnableInputs("XRI LeftHand Interaction");
            m_HandlingInputs.EnableInputs("XRI RightHand Interaction");
            m_HandlingInputs.EnableInputs("XRI LeftHand Locomotion");
            m_HandlingInputs.EnableInputs("XRI RightHand Locomotion");
            m_InputsEnabled = true;
        }

        if (!m_GrabHapticDone)
        {
            m_Interactors[0].SendHapticImpulse(m_HapticStrength, m_HapticDuration);
            m_Interactors[1].SendHapticImpulse(m_HapticStrength, m_HapticDuration);
            m_GrabHapticDone = true;
        }
    }

    void ActionBasedMovements()
    {
        if (m_NumOfTimeInMouvement < m_NumOfTimeInMouvementObjective)
        {
            if (IsControllerButtonDown(m_LeftController, UnityEngine.XR.CommonUsages.primary2DAxisTouch))
                if (!m_IsMoving) m_IsMoving = true;
                else if (m_IsMoving) m_IsMoving = false;
        }
        else if (m_NumOfTimeInMouvement >= m_NumOfTimeInMouvementObjective)
        {
            if (!m_ResetDialogueDone)
            {
                m_Dialogues = new string[1];
                m_Dialogues[0] = "";
                m_SoundPlayer.SetDialogues(m_Dialogues);


                m_SoundPlayer.StopAnyAudioPlaying();
                ClearHighlightList();
                m_IsMoving = false;
                m_AddRenderers = true;
                m_Stage = TutorialStages.Teleportation;
                m_ResetDialogueDone = true;
            }
        }

        if (m_IsMoving)
        {
            m_NumOfTimeInMouvement += Time.deltaTime;
        }
    }

    void TeleportationBasedMovements()
    {
        if (m_NumOfTeleportations >= m_NumOfTeleportationsObjective)
        {
            if (!m_ResetDialogueDone)
            {
                m_Dialogues = new string[1];
                m_Dialogues[0] = "";
                m_SoundPlayer.SetDialogues(m_Dialogues);

                m_SoundPlayer.StopAnyAudioPlaying();
                ClearHighlightList();
                foreach (var door in m_Doors)
                {
                    door.SetReadyToBeOpen(true);
                }

                m_Stage = TutorialStages.Grab;
                m_ResetDialogueDone = true;
                m_AddRenderers = true;
            }
        }
    }

    IEnumerator GrabbingObject()
    {
        if (m_NumOfGrabs < m_NumOfGrabObjective)
        {
            m_Tablets.GetComponent<Highlight>().IsActive(true);

            if (IsControllerButtonDown(m_RightController, UnityEngine.XR.CommonUsages.gripButton) ||
                IsControllerButtonDown(m_LeftController, UnityEngine.XR.CommonUsages.gripButton))
            {
                foreach (var controller in m_Interactors)
                    if (controller.hasSelection) m_IsGrabbing = true;
            }
            
            if (m_IsGrabbing)
            {
                m_IsGrabbing = false;
                m_NumOfGrabs++;
            }
        }
        else if (m_NumOfGrabs >= m_NumOfGrabObjective)
        {
            if (!m_ResetDialogueDone)
            {
                m_Dialogues = new string[1];
                m_Dialogues[0] = "";
                m_SoundPlayer.SetDialogues(m_Dialogues);
                m_ResetDialogueDone = true;
            }

            if (!m_GrabDialoguePart3Done)
            {

                m_Dialogues = new string[2];
                m_Dialogues[0] = "The tablet works as your menu and is always stored on your lower back.";
                m_Dialogues[1] = "Now it's time to suit up, head over to the table and gear up.";
                m_SoundPlayer.SetDialogues(m_Dialogues);
                m_SoundPlayer.PlayDisk("Tablet", 0);
                m_GrabDialoguePart3Done = true;
            }
            yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
/*            yield return new WaitUntil(() => m_TabletAnchor.hasSelection);

            m_Stage = TutorialStages.Done;*/

            /*m_Tablets.GetComponent<Highlight>().IsActive(false);
            
            ClearHighlightList();
            m_AddRenderers = true;
            
            if (!m_FinalDialogueDone)
            {
                m_Dialogues = new string[2];
                m_Dialogues[0] = "Amazing! Your tablet will always be located there and can be accessed at all time.";
                m_Dialogues[1] = "Let’s meet the other crewmates and get yourself in the spacesuit.";
                m_SoundPlayer.SetDialogues(m_Dialogues);
                m_SoundPlayer.PlayDisk("Tutorial_Ending", 0);
                m_FinalDialogueDone = true;

                m_Spacesuit.GetComponent<Highlight>().IsActive(true);
            }
            yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());*/
        }
    }

    void StartingTutorial()
    {
        if (m_TimerBeforeTutorial > 0.0f)
            m_TimerBeforeTutorial -= Time.deltaTime;
        else
        {
            if (m_Stage == TutorialStages.NotActive)
            {
                m_Stage = TutorialStages.BasicMovements;
                m_AddRenderers = true;
                m_HandlingInputs.DisableInputs("XRI LeftHand Locomotion");
                m_HandlingInputs.DisableInputs("XRI RightHand Locomotion");
                m_HandlingInputs.DisableInputs("XRI LeftHand Interaction");
                m_HandlingInputs.DisableInputs("XRI RightHand Interaction");
                m_TutorialHasBegan = false;
            }
        }
    }

    private void TeleportActivated(InputAction.CallbackContext obj)
    {
        if (SceneManager.GetActiveScene().name == "Scenario_1")
            m_NumOfTeleportations++;
    }

    bool IsControllerButtonDown(List<UnityEngine.XR.InputDevice> controller, UnityEngine.XR.InputFeatureUsage<bool> button)
    {
        bool triggerValue;
        if (controller.Count > 0)
            return controller[0].TryGetFeatureValue(button, out triggerValue) && triggerValue;
        else return false;
    }

    private void AddRenderer(Renderer[] renderer, Highlight controllerToHighlight)
    {
        foreach (var rend in renderer)
        {
            if (m_AddRenderers)
                controllerToHighlight.AddRenderers(rend);
        }
        m_AddRenderers = false;
    }

    void ClearHighlightList()
    {
        foreach (var highlight in m_HighlightsList)
        {
            highlight.ClearLists();
        }
    }

    public void TutorialHasBegan()
    {
        m_TutorialHasBegan = true;
    }

    public bool IsTutorialTriggered()
    {
        return m_TutorialIsTrigged;
    }

    public bool HasFinishedDialogue()
    {
        return m_HasFinishedDialogue;
    }

    public bool HasFinishedFinalDialogue()
    {
        return m_FinalDialogueDone;
    }

    public void SkipTutorial()
    {
        ClearHighlightList();
        m_SoundPlayer.StopAnyAudioPlaying();
        m_HasFinishedDialogue = false;
        m_Stage = TutorialStages.Done;
    }

    enum TutorialStages
    {
        NotActive,
        BasicMovements,
        Teleportation,
        Grab,
        Done,
    }
}
