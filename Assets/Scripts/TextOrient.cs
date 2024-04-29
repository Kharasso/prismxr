using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOrient : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        reorient(playerCamera.transform, gameObject);
        
    }

    private void reorient(Transform target, GameObject source)
    {
        Vector3 diff = target.position - source.transform.position;
        Vector3 newForward = new Vector3(diff.x, source.transform.forward.y, diff.z);
        source.transform.forward = -newForward;
    }
}
