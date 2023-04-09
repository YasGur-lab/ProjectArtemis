using UnityEngine;

/*************************************
Class: IsGroundedRaycast
Author: Antoine Plouffe
Created: 01/09/2023
Last Modified: 02/12/2022
Description: This class is in charge of calculating if the player is close to the ground.

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

public class IsGroundedRaycast : MonoBehaviour
{
    public bool IsGroundedByRaycast(GameObject go, float distance)
    {
        if (Physics.Raycast(go.transform.position, Vector3.down, out RaycastHit hit, distance))
        {
            return true;
            //if (hit.collider.gameObject.tag == "Terrain")
            //{
            //    ////Debug.Log(hit.distance);
            //    //Debug.Log(true);
            //    return true;
            //}
        }
        return false;
    }
}
