using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: MissionManager
Author: Alexander Achorn
Created: 17/02/2022
Last Modified: 
Description: This class is in charge of managing all missions.

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

public class MissionManager : MonoBehaviour, ISaveableList
{
    public List<Mission> missions;
    private List<bool> missionStates;

    //Save & Load
    public List<bool> SaveStates()
    {
        return missionStates;
    }

    public void LoadStates(List<bool> states)
    {
        missionStates = states;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Mission mission in missions)
        {
            mission.Update();
        }
    }

}
