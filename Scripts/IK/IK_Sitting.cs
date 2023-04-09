using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Sitting : MissionObject
{
    [SerializeField] private Transform cameraOffset;
    [SerializeField] private GameObject lowerRig;
    [Header("Left Leg")]
    [SerializeField] private Transform leftThigh;
    [SerializeField] private Transform leftCalf;
    [SerializeField] private Transform leftfoot;
    [Header("Right Leg")]
    [SerializeField] private Transform rightThigh;
    [SerializeField] private Transform rightCalf;
    [SerializeField] private Transform rightfoot;


    // Update is called once per frame
    void Update()
    {
        if(isTriggered)
        {
            lowerRig.SetActive(false);
            cameraOffset.localPosition = new Vector3(0, -0.55f, 0);
            leftThigh.rotation = new Quaternion(-0.774077594f, 0.618301749f, 0.0518945381f, -0.125753254f);
            leftCalf.rotation = new Quaternion(0.0445494987f, -0.12339738f, 0.715396523f, 0.686291754f);
            leftfoot.rotation = new Quaternion(0.0376925208f, 0.00361717422f, -0.497710377f, 0.866516411f);

            rightThigh.rotation = new Quaternion(-0.785308719f, 0.615255892f, -0.0664513931f, -0.0182958525f);
            rightCalf.rotation = new Quaternion(-0.05010508f, -0.0168804098f, 0.728569865f, 0.682928026f);
            rightfoot.rotation = new Quaternion(-0.0377116203f, -0.00340115791f, -0.497774184f, 0.866479754f);
        }

        if(!isTriggered && isComplete)
        {
            lowerRig.SetActive(true);
            cameraOffset.position = new Vector3(0, 0, 0);
        }
    }
}
