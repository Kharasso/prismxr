using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPosition : MonoBehaviour
{
    public GameObject track;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
/*        foreach (Transform child in transform)
        {
            //child is your child transform
            foreach (Transform childchild in child.transform)
            {
                Debug.Log(childchild.name);
                Debug.Log(childchild.transform.position);
            }
                
        }*/
        gameObject.transform.rotation = Camera.main.transform.rotation;
        

        gameObject.transform.position = Camera.main.transform.position;
/*        Debug.Log(gameObject.transform.position);*/
/*        Debug.Log(track.transform.position);*/
    }
}
