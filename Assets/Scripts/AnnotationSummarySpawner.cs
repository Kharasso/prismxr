using FlexXR.Runtime.FlexXRPanel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class AnnotationSummarySpawner : MonoBehaviour
{
    public GameObject prefab;

    public void instantiateAnnotationUI(GameObject caller)
    {
        

        Transform spawnSpot = caller.transform.Find("Spawn Spot");

        Vector3 newViewForward = -spawnSpot.forward;
        Debug.Log(newViewForward);
        Vector3 newViewPosition = spawnSpot.position - newViewForward * 1.2f;

        GameObject.Find("XR Rig").transform.forward = newViewForward;
        GameObject.Find("XR Rig").transform.position = newViewPosition;

        

        string targetName = "AnnotationSummary";

        Debug.Log(targetName);
        Debug.Log(GameObject.Find(targetName));

        if (GameObject.Find(targetName))
        {
            return;
        }

        var newObject = (GameObject) Instantiate(prefab);
        newObject.transform.position = spawnSpot.position + new Vector3(0, 1.2f, 0);
/*        newObject.transform.forward = -newViewForward;*/

        /*        var newObject = (GameObject)Instantiate(prefab);*/
        newObject.name = targetName;
        SummaryDataManager objDataManager =  newObject.transform.Find("SummaryDataManager").GetComponent<SummaryDataManager>();

        Debug.Log(objDataManager);

        objDataManager.rootVisualElement = (VisualElement) newObject.GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;

    }

    public void destroyAnnotationUI(GameObject caller)
    {
        caller.SetActive(false);
        Destroy(caller);
    }


}
