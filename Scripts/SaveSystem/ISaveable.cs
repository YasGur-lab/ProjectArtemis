using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: ISaveable
Author: Antoine Plouffe
Created: 01/09/2023
Last Modified: 02/12/2022
Description: This interface is in charge of adding the Save and Load state to any gameObject that needs it.

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
public interface ISaveable
{
    object SaveState();
    void LoadState(object state);
}
