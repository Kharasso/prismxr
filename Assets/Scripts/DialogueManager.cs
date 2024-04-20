using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using FlexXR.Runtime.FlexXRPanel;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [Header("Virtual Guide Dialog")]
    [SerializeField] private GameObject dialogueView;

    private GameObject dialoguePanel;
    private static DialogueManager instance;
    private Story currStory;
    private bool isPlaying;
    private VisualElement rootVisualElement;
    private Label dialogueLabel;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Singleton class - more than one created");
        }
        instance = this;

        dialoguePanel = dialogueView.transform.Find("World Content").transform.Find("Flex Mesh").gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        rootVisualElement = (VisualElement) dialogueView.GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;
        dialogueLabel = rootVisualElement.Q<Label>("DialogueLabel");
        dialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlaying)
        {
            return;
        }


    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    public void TriggerDialogue(TextAsset dialogueContent)
    {
        currStory = new Story(dialogueContent.text);
        isPlaying = true;
        dialoguePanel.SetActive(true);
        /*        dialogueLabel.text = dialogueContent.text;*/
        ContinueStory();

        /*if (currStory.canContinue)
        {
            Debug.Log("continue");
        }
        else
        {
            ExitDialogue();
        }*/
    }

    private void ExitDialogue()
    {
        dialoguePanel.SetActive(false);
        isPlaying = false;
    }

    public void ContinueStory()
    {
        if (currStory.canContinue)
        {
            dialogueLabel.text = currStory.Continue();
        }
        else
        {
            ExitDialogue();
        }
    }
}
