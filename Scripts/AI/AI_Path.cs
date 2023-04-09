using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Path : MonoBehaviour
{
    private const float m_WaypointGizmoRadius = 0.3f;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextWaypoint(i);

            Gizmos.DrawSphere(GetWaypoint(i), m_WaypointGizmoRadius);
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
        }
    }

    public int GetNextWaypoint(int i)
    {
        if (i + 1 == transform.childCount)
            return i;
        return i + 1;
    }

    public Vector3 GetWaypoint(int i)
    {
        return transform.GetChild(i).position;
    }

    public Vector3 GetFinalWaypoint(int i)
    {
        if (i + 1 == transform.childCount)
            return transform.GetChild(i).position;
        else
            return Vector3.zero;
    }

    public int GetPathLength()
    {
        return transform.childCount;
    }
}
