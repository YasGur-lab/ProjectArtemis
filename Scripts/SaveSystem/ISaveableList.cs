using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveableList
{
    List<bool> SaveStates();
    void LoadStates(List<bool> states);
}
