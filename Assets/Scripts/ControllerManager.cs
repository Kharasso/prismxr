using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerManager : MonoBehaviour
{
    [SerializeField] private XRRayInteractor LeftInteractor;
    [SerializeField] private XRRayInteractor RightInteractor;

    private static ControllerManager instance;
    // enter ui

    private XRRayInteractor.LineType defaultLineType;
    private InteractionLayerMask defaultLayerMask;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Singleton class - more than one created");
        }
        instance = this;
    }

    void Start()
    {
        defaultLineType = LeftInteractor.lineType;
        defaultLayerMask = LeftInteractor.interactionLayers;
    }

    public static ControllerManager GetInstance()
    {
        return instance;
    }

    public void EnterUI()
    {
        LeftInteractor.interactionLayers = InteractionLayerMask.GetMask("ui");
        RightInteractor.interactionLayers = InteractionLayerMask.GetMask("ui");
        LeftInteractor.lineType = XRRayInteractor.LineType.StraightLine;
        RightInteractor.lineType = XRRayInteractor.LineType.StraightLine;
    }

    public void ExitUI()
    {
        LeftInteractor.interactionLayers = defaultLayerMask;
        RightInteractor.interactionLayers = defaultLayerMask;
        LeftInteractor.lineType = defaultLineType;
        RightInteractor.lineType = defaultLineType;
    }
}
