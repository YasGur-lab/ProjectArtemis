using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario1_Car_Sequence : MonoBehaviour
{
    [SerializeField] GameObject m_ThisCar;
    [SerializeField] GameObject m_MainCar;

    AI_Path m_ToGoOutElevatorPath;
    // Start is called before the first frame update

    private void Update()
    {
        if (m_MainCar.GetComponent<Car_Controller>().IsPlayerInCar())
            m_ThisCar.GetComponent<Car_Controller>().SetCarToStart();
    }
}
