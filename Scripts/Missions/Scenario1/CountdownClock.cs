using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*************************************
Class: TabletMenuScript
Author: Noah Maxwell
Created: 07/04/2023
Last Modified: 07/04/2023
Description: This class is controls a countdown timer on a canvas

Change Log
**********
Date: 07/04/2023
Author: Noah Maxwell
Verified By:
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class CountdownClock : MonoBehaviour
{
    [SerializeField] TMP_Text TimeText;
    [SerializeField] float StartTime;

    private float CurrentTime;
    private float AccumulatedTime;

    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = StartTime;
        TimeText.text = TimeToString(StartTime);
    }

    // Update is called once per frame
    void Update()
    {
        AccumulatedTime += Time.deltaTime;

        if (AccumulatedTime >= 1.0f)
        {
            CurrentTime -= AccumulatedTime;
            AccumulatedTime = 0.0f;

            TimeText.text = TimeToString(CurrentTime);
        }
    }

    private string TimeToString(float timeInSeconds)
    {
        string seconds;
        string minutes;
        string hours;

        hours = Mathf.Floor(timeInSeconds / 3600.0f).ToString();
        minutes = Mathf.Floor(timeInSeconds % 3600.0f / 60.0f).ToString();
        seconds = Mathf.Floor(timeInSeconds % 3600.0f % 60.0f).ToString();

        if(Mathf.Floor(timeInSeconds / 3600.0f) < 10.0f)
        {
            hours = "0" + hours;
        }
        if (Mathf.Floor(timeInSeconds % 3600.0f / 60.0f) < 10.0f)
        {
            minutes = "0" + minutes;
        }
        if (Mathf.Floor(timeInSeconds % 3600.0f % 60.0f) < 10.0f)
        {
            seconds = "0" + seconds;
        }

        return "<mspace=3.50>T- " + hours + " : " + minutes + " : " + seconds + "</mspace>";
    }
}


