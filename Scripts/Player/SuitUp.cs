using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: MissionObject
Author: Alexander Achorn
Created: 17/02/2022
Last Modified: 
Description: This class is meant to be inherited from to assure all events in a mission have consistant Complete and Triggered states.

Change Log
**********
Date: 17/02/2022
Author: Alexander Achorn
Verified By:
Changes: Created

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

[System.Serializable]
public class SuitUp : MissionObject
{
    [Header("Suit Pieces")]
    [SerializeField] GameObject helmet;
    [SerializeField] GameObject leftGlove;
    [SerializeField] GameObject rightGlove;
    [SerializeField] GameObject pumpkinSuit;
    [Header("Moon Suit")]
    [SerializeField] GameObject moonSuit;
    [SerializeField] GameObject leftFlashlight;
    [SerializeField] GameObject rightFlashlight;
    [Header("Body Pieces")]
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;
    [Header("Settings")]
    [SerializeField] bool onMoon;
    [SerializeField] GravityController gravityController;
    [SerializeField] bool fullSuit;
    [SerializeField] bool helmetOn;
    [SerializeField] bool leftGloveOn;
    [SerializeField] bool rightGloveOn;
    [SerializeField] bool pumpkinSuitOn;
    [Space]
    [SerializeField] bool flashLight = false;

    bool suitedUp = false;


    // Start is called before the first frame update
    void Start()
    {
        leftFlashlight.SetActive(false);
        rightFlashlight.SetActive(false);

        CheckSuitState();
    }

    // Update is called once per frame
    void Update()
    {
        if(!suitedUp)
        {
            CheckSuitState();
        }
        else if(!isComplete)
        {
            isComplete = true;
        }

        if(flashLight)
        {
            leftFlashlight.SetActive(true);
            rightFlashlight.SetActive(true);
        }
    }

    void CheckSuitState()
    {
        if (onMoon)
        {
            //fullSuit = true;
            gravityController.SetMoonGravity();
        }

        if (fullSuit)
        {
            helmetOn = false;
            leftGloveOn = false;
            rightGloveOn = false;
            pumpkinSuitOn = false;

            moonSuit.SetActive(true);
            leftFlashlight.SetActive(true);
            rightFlashlight.SetActive(true);

            helmet.SetActive(false);
            leftGlove.SetActive(false);
            rightGlove.SetActive(false);
            pumpkinSuit.SetActive(false);

            head.SetActive(false);
            body.SetActive(false);
            leftHand.SetActive(false);
            rightHand.SetActive(false);

            suitedUp = true;
        }
        else
        {
            if (helmetOn)
            {
                helmet.SetActive(true);
                head.SetActive(false);
            }
            if (leftGloveOn)
            {
                leftGlove.SetActive(true);
                leftHand.SetActive(false);
            }
            if (rightGloveOn)
            {
                rightGlove.SetActive(true);
                rightHand.SetActive(false);
            }
            if (pumpkinSuitOn)
            {
                pumpkinSuit.SetActive(true);
                body.SetActive(false);
            }

            if(helmetOn && leftGloveOn && rightGloveOn && pumpkinSuitOn)
            {
                suitedUp = true;
            }
        }
    }

    public void PutOnHelmet()
    {
        helmetOn = true;
    }

    public void PutOnLeftGlove()
    {
        leftGloveOn = true;
    }

    public void PutOnRightGlove()
    {
        rightGloveOn = true;
    }

    public void PutOnSuit()
    {
        pumpkinSuitOn = true;
    }
}
