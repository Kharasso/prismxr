using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Hint")]
    [SerializeField] private GameObject dialogueHint;

    [Header("Dialog Content")]
    [SerializeField] private TextAsset dialogueContent;

    private bool playIsNear;
    private void Awake()
    {

        playIsNear = false;
        dialogueHint.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (playIsNear)
        {
            dialogueHint.SetActive(true);
        }
        else
        {
            dialogueHint.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered");
            playIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playIsNear = false;
        }
    }

    public void ShowDialogue()
    {
        if (playIsNear)
        {
            DialogueManager.GetInstance().TriggerDialogue(dialogueContent);
        }
    }
}
