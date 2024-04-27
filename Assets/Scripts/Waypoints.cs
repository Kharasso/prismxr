using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        foreach(Transform child in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(child.position, 0.5f);
        }

        Gizmos.color = Color.yellow;

        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild((i + 1) % transform.childCount).position);
        }
    }

    public Transform GetNextWaypoint(Transform currWP)
    {
        if (currWP == null)
        {
            return transform.GetChild(0);
        }

        return transform.GetChild((currWP.GetSiblingIndex() + 1) % transform.childCount);
    }

}

