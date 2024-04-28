using FlexXR.Runtime.FlexXRPanel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class VirtualGuestbookSpawner : MonoBehaviour
{
    public GameObject prefab;

    public void instantiateGuestbookUI(GameObject caller)
    {
        

        Transform spawnSpot = caller.transform.Find("Spawn Spot");

        Vector3 newViewForward = -spawnSpot.forward;
        Debug.Log(newViewForward);
        Vector3 newViewPosition = spawnSpot.position - newViewForward * 1.2f;

        GameObject.Find("XR Rig").transform.forward = newViewForward;
        GameObject.Find("XR Rig").transform.position = newViewPosition;

        

        string targetName = "GuestbookUI";

        Debug.Log(targetName);
        Debug.Log(GameObject.Find(targetName));

/*        ControllerManager.GetInstance().EnterUI();*/

        if (GameObject.Find(targetName))
        {
            GameObject target = GameObject.Find(targetName);
            target.SetActive(true);
            return;
        }

        var newObject = (GameObject) Instantiate(prefab);
        newObject.transform.position = spawnSpot.position + new Vector3(0, 1.2f, 0);
/*        newObject.transform.forward = -newViewForward;*/

        /*        var newObject = (GameObject)Instantiate(prefab);*/
        newObject.name = targetName;
        GuestbookDataManager objDataManager =  newObject.transform.Find("GuestbookDataManager").GetComponent<GuestbookDataManager>();

        Debug.Log(objDataManager);

        objDataManager.rootVisualElement = (VisualElement) newObject.GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;

    }

    public void destroyAnnotationUI(GameObject caller)
    {

        caller.SetActive(false);
/*        ControllerManager.GetInstance().ExitUI();*/
        /*        Destroy(caller);*/
    }


}
