using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOutOfScope : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject mainCameraOffset;
    public GameObject canvas;

    void Start()
    {
        mainCameraOffset = GameObject.Find("Camera Offset");
       
        /*        Debug.Log(mainCameraOffset.name);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas.activeSelf && mainCameraOffset != null)
        {
            float canvasCameraDistance = (canvas.transform.position - mainCameraOffset.transform.position).magnitude;

            if (canvasCameraDistance >= 3.0)
            {
/*                ControllerManager.GetInstance().ExitUI();*/
                Destroy(canvas);
            }
        }
    }
}
