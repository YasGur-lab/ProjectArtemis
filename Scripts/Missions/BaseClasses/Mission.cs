using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: Mission
Author: Alexander Achorn
Created: 17/02/2022
Last Modified: 
Description: This class is in charge of holding all information related to a mission.

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
public class Mission
{
    [SerializeField] string missionName;
    [SerializeField] bool isComplete;
    [SerializeField] bool hasStartTrigger;
    [SerializeField] MissionObject startTrigger;
    [SerializeField] MissionObject[] eventOrder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (!isComplete)
        {
            if (hasStartTrigger)
            {
                if (startTrigger)
                {
                    /*if(!startTrigger.isTriggered)
                    {
                        startTrigger.isTriggered = true;
                    }*/

                    if (!startTrigger.isComplete)
                    {
                        return;
                    }

                    int completeCount = 0;

                    foreach (MissionObject obj in eventOrder)
                    {
                        if (obj.isComplete) completeCount++;
                    }

                    if (completeCount == eventOrder.Length) isComplete = true;
                }
            }

            if ((!hasStartTrigger || startTrigger.isComplete) && eventOrder.Length > 0)
            {
                if (!eventOrder[0].isTriggered && !eventOrder[0].isComplete)
                {
                    eventOrder[0].isTriggered = true;
                }
                else if (eventOrder[0].isComplete)
                {
                    for (int i = 1; i < eventOrder.Length; i++)
                    {
                        if (eventOrder[i - 1].isComplete)
                        {
                            if (!eventOrder[i].isTriggered && !eventOrder[i].isComplete)
                            {
                                eventOrder[i].isTriggered = true;
                            }
                        }
                    }
                }
            }
        }
    }

    bool GetMissionState()
    {
        return isComplete;
    }
}
