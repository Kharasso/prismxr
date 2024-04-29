using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using FlexXR.Runtime.FlexXRPanel;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

public class IntroManager : MonoBehaviour
{
    [Header("Intro Dialog")]
    [SerializeField] private GameObject IntroView;

    [Header("Player Object")]
    public GameObject playerCamera;

    [Header("Dialog Content")]
    [SerializeField] private TextAsset dialogueContent;

    private GameObject dialoguePanel;
    private static IntroManager instance;
    private Label dialogueLabel;
    private Label titleLabel;
    private Story currStory;
    public bool isPlaying;
    private VisualElement rootVisualElement;
    private VisualElement choiceContainer;
    private Button continueButton;
    private Button closeButton;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Singleton class - more than one created");
        }
        instance = this;

 
    }
    // Start is called before the first frame update
    void Start()
    {
        dialoguePanel = IntroView.transform.Find("World Content").transform.Find("Flex Mesh").gameObject;
        /*        Debug.Log(dialogueView.GetComponent<FlexXRPanelManager>());*/
        if (rootVisualElement == null)
        {
            rootVisualElement = (VisualElement)IntroView.GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;
        }
            
        dialogueLabel = rootVisualElement.Q<Label>("DialogueLabel");
        titleLabel = rootVisualElement.Q<Label>("HeaderText");
        choiceContainer = rootVisualElement.Q<VisualElement>("ScrollChoices");
        continueButton = rootVisualElement.Q<Button>("ContinueButton");
        closeButton = rootVisualElement.Q<Button>("CloseButton");

        // there is no choice in this dialogue, hide all the choice buttons
        for (int i = 0; i < choiceContainer.childCount; i++)
        {
            rootVisualElement.Q<Button>("Choice" + (i + 1).ToString()).style.display = DisplayStyle.None;
        }

        /*        Vector3 newDialoguePosition = dialogueView.transform.position;
                newDialoguePosition.y = playerCamera.transform.position.y;
                dialogueView.transform.position = newDialoguePosition;*/
        titleLabel.text = "Introduction";
        isPlaying = false;
        dialoguePanel.SetActive(false);
        continueButton.clicked += ContinueStory;
        closeButton.clicked += OnClickClose;

        TriggerDialogue(dialogueContent);


        /*        choiceButtons[0].clicked += delegate { OnClickChoiceButton(0); };
                choiceButtons[1].clicked += delegate { OnClickChoiceButton(1); };
                choiceButtons[2].clicked += delegate { OnClickChoiceButton(2); };
                choiceButtons[3].clicked += delegate { OnClickChoiceButton(3); };
                choiceButtons[4].clicked += delegate { OnClickChoiceButton(4); };*/
        /*Debug.Log(choiceContainer.childCount);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            return;
        }
    }

    public static IntroManager GetInstance()
    {
        return instance;
    }

    void OnClickClose()
    {
        /*        Destroy(panel) ;*/
        dialoguePanel.SetActive(false);
        isPlaying = false;
        /*        ControllerManager.GetInstance().ExitUI();*/
    }

    public void ReForward(Transform target, GameObject source)
    {
        Vector3 diff = target.transform.position - source.transform.position;
        Vector3 newForward = new Vector3(diff.x, source.transform.forward.y, diff.z);
        source.transform.forward = newForward;
    }

    public void TriggerDialogue(TextAsset dialogueContent)
    {
        /*        ControllerManager.GetInstance().EnterUI();*/
        currStory = new Story(dialogueContent.text);
        isPlaying = true;
        dialoguePanel.SetActive(true);
        /*        Vector3 diff = playerCamera.transform.position - guideAvatar.transform.position;
                Vector3 newForward = new Vector3(diff.x, guideAvatar.transform.forward.y, diff.z);
                guideAvatar.transform.forward = newForward;*/
        /*        guideAvatar.transform.right = playerCamera.transform.position - guideAvatar.transform.position;*/
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

        /*        Debug.Log(currStory.state.currentPointer);
                Debug.Log(currStory.state.currentTags);
                Debug.Log(currStory.state.currentFlowName);*/
        if (currStory.canContinue)
        {
            dialogueLabel.text = currStory.Continue();
            continueButton.style.display = DisplayStyle.Flex;
        }
        else
        {
            ExitDialogue();
        }
    }


}
