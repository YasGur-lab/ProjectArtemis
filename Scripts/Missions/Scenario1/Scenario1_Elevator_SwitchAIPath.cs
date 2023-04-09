using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario1_Elevator_SwitchAIPath : MonoBehaviour
{
    [SerializeField] Controller_GoingToElevator_Scenario m_GoingToElevator_Scenario;
    [SerializeField] Elevator m_Elevator;
    [SerializeField] Scenario1_Elevator_Sequence m_InElevator_Scenario;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(m_Elevator.IsReadyToGoDown())
        //    m_GoingToElevator_Scenario.SwitchPath();

        //if (m_Elevator.IsAtCarFloor())
        //    m_InElevator_Scenario.SwitchPath();
    }
}
