using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOpener : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    public void openCanvas()
    {
        Debug.Log("invoked");
        if(canvas != null)
        {
            canvas.SetActive(true);
        }
        Debug.Log("Finished");
    }
}
