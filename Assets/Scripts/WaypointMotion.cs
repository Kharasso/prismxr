using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Motion : MonoBehaviour
{
    [Header("Waypoint Motion System")]
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private float speed = 1.2f;
    [SerializeField] private float distThreshold = 0.1f;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Animator animator;
/*    [SerializeField] private GameObject playerCamera;*/

    private Transform currWP;

    // Start is called before the first frame update
    void Start()
    {
        // initialize position of virtual guide to the first waypoint
        currWP = waypoints.GetNextWaypoint(currWP);

        transform.position = currWP.position;
        /*        guideAvatar.transform.LookAt(playerCamera.transform);*/
        /*        guideAvatar.transform.right = playerCamera.transform.position - guideAvatar.transform.position;*/
        /*        Vector3 diff = playerCamera.transform.position - guideAvatar.transform.position;
                Vector3 newForward = new Vector3 (diff.x, guideAvatar.transform.forward.y, diff.z);
                guideAvatar.transform.forward = newForward;*/
        dialogueManager.ReForward(dialogueManager.playerCamera.transform, dialogueManager.guideAvatar);
        dialogueManager.ReForward(dialogueManager.playerCamera.transform, dialogueManager.panelContainer);

    }

    // Update is called once per frame
    void Update()
    {  
        // only engage in movement when dialogue is not playing
        if (!dialogueManager.isPlaying)
        {

            if (Vector3.Distance(transform.position, currWP.position) < distThreshold)
            {
                currWP = waypoints.GetNextWaypoint(currWP);
            }


            // only move when the current animator state is walking
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("walking"))
            {
                /*guideAvatar.transform.LookAt(currWP);*/
                dialogueManager.ReForward(currWP, dialogueManager.guideAvatar);
                dialogueManager.ReForward(currWP, dialogueManager.panelContainer);
                /*                guideAvatar.transform.right = currWP.position - guideAvatar.transform.position;*/
                transform.position = Vector3.MoveTowards(transform.position, currWP.position, speed * Time.deltaTime);
            }
            
        }
        
    }
}
