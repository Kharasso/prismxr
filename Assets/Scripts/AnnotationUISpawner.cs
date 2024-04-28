using FlexXR.Runtime.FlexXRPanel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class AnnotationUISpawner : MonoBehaviour
{
    public GameObject prefab;
    [SerializeField] private GameObject rig;

    public void instantiateAnnotationUI(GameObject caller)
    {
        

        Transform spawnSpot = caller.transform.Find("Spawn Spot");
        Transform displayCase = caller.transform.Find("Display Case 2");
        Vector3 newViewPosition = new Vector3(spawnSpot.position.x, rig.transform.position.y, spawnSpot.position.z);

        rig.transform.position = newViewPosition;

        reorient(displayCase, rig);

/*        Vector3 newViewForward = -rig.transform.forward;*/
        /*        Debug.Log(newViewForward);*/
        /*        Vector3 newViewPosition = spawnSpot.position - newViewForward * 1.2f;*/
        

        /*GameObject.Find("XR Rig").transform.forward = newViewForward;*/
       

        

        string targetName = "AnnotationUI" + caller.name;

        Debug.Log(targetName);
        Debug.Log(GameObject.Find(targetName));
/*        ControllerManager.GetInstance().EnterUI();*/

        if (GameObject.Find(targetName))
        {
            return;
        }

        var newObject = (GameObject) Instantiate(prefab);
        newObject.transform.position = spawnSpot.position + rig.transform.forward * 1.2f;

        Vector3 newObjectPosition = spawnSpot.position / 2.0f + displayCase.position / 2.0f;
        newObject.transform.position = new Vector3(newObjectPosition.x, 1.2f, newObjectPosition.z);

        reorient(rig.transform, newObject);
        newObject.transform.forward = -newObject.transform.forward;

/*        Vector3 diff = playerCamera.transform.position - newObject.transform.position;
        Vector3 newForward = new Vector3(diff.x, newObject.transform.forward.y, diff.z);
        newObject.transform.forward = newForward;*/

        /*        newObject.transform.forward = -newViewForward;*/

        /*        var newObject = (GameObject)Instantiate(prefab);*/
        newObject.name = targetName;
        ObjectDataManager objDataManager =  newObject.transform.Find("ObjectDataManager").GetComponent<ObjectDataManager>();

        Debug.Log(objDataManager);

        objDataManager.rootVisualElement = (VisualElement) newObject.GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;

    }

    private void reorient(Transform target, GameObject source)
    {
        Vector3 diff = target.position - source.transform.position;
        Vector3 newForward = new Vector3(diff.x, source.transform.forward.y, diff.z);
        source.transform.forward = newForward;
    }

    public void destroyAnnotationUI(GameObject caller)
    {
/*        ControllerManager.GetInstance().ExitUI();*/
        caller.SetActive(false);
        Destroy(caller);

    }


}
