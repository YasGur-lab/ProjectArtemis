using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Class: ScrewBucket
Author: Charles Weir 
Created: 3 / 15 / 2023

Description: This class is in charge of the ScrewBucket.

 
 
 Change Log
**********
Date: 3 / 15 / 2023
Author: Charles Weir 
Verified By: 
Changes: Created.

*************************************/



public class ScrewBucket : MonoBehaviour
{

    public List<Screw> Screws;
    List<float> LerpPercentages;
    List<Vector3> StartPositions;
    //float LerpDuration = 1.69f;
    public Collider bucketArea;
    public GameObject ScrewLerpPoint;
    // Start is called before the first frame update
    void Start()
    {
        LerpPercentages = new List<float>();
        StartPositions = new List<Vector3>();

        for (int i = 0; i < Screws.Count; i++)
        {
            LerpPercentages.Add(0.0f);
            StartPositions.Add(Vector3.zero);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Screws.Count; i++)
        {
/*            if(Screws[i].IsInBucket)
            {
                float dist = Vector3.SqrMagnitude(ScrewLerpPoint.transform.position - Screws[i].transform.position);
                if(dist > 0.69)
                {
                    Screws[i].IsInBucket = false;
                }
            }
            if (!Screws[i].IsInBucket && Screws[i].ShouldBeReturned)
            {

                if (LerpPercentages[i] == 0)
                {
                    StartPositions[i] = Screws[i].transform.position;
                }
                else if (LerpPercentages[i] > 1.0f)
                {
                    Screws[i].IsInBucket = true;
                    Screws[i].transform.position = ScrewLerpPoint.transform.position;
                    LerpPercentages[i] =0.0f;
                }
                else
                {

                    Screws[i].transform.position = Vector3.Lerp(StartPositions[i], ScrewLerpPoint.transform.position, LerpPercentages[i]);
                }
                LerpPercentages[i] += Time.deltaTime / LerpDuration;
            }*/
        }
    }
}
