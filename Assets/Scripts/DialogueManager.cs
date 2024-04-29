using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using FlexXR.Runtime.FlexXRPanel;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

public class DialogueManager : MonoBehaviour
{
    [Header("Virtual Guide Dialog")]
    [SerializeField] private GameObject dialogueView;
    [SerializeField] private GameObject mapView;

    [Header("Player Object")]
    public GameObject playerCamera;

    private GameObject dialoguePanel;
    private GameObject mapPanel;
    private static DialogueManager instance;
    private Story currStory;
    public bool isPlaying;
    private VisualElement rootVisualElement;
    private VisualElement rootVisualElementMap;
    private Label dialogueLabel;
    private Button continueButton;
    private Button closeButton;
    private Button closeButtonMap;
    private VisualElement choiceContainer;
    private List<Button> choiceButtons;
    private string LastKnot;

    [Header("Avatar Animator")]
    public GameObject panelContainer;
    public GameObject guideAvatar;
    public Animator AvatarAnimator;
    
    

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

        dialoguePanel = dialogueView.transform.Find("World Content").transform.Find("Flex Mesh").gameObject;
        mapPanel = mapView.transform.Find("World Content").transform.Find("Flex Mesh").gameObject;
        if (rootVisualElement == null)
        {
            rootVisualElement = (VisualElement)dialogueView.GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;
        }

        if (rootVisualElementMap == null)
        {
            rootVisualElementMap = (VisualElement)mapView.GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;
        }

        dialogueLabel = rootVisualElement.Q<Label>("DialogueLabel");
        continueButton = rootVisualElement.Q<Button>("ContinueButton");
        closeButton = rootVisualElement.Q<Button>("CloseButton");
        closeButtonMap = rootVisualElementMap.Q<Button>("CloseButton");
        choiceContainer = rootVisualElement.Q<VisualElement>("ScrollChoices");
        choiceButtons = new List<Button>();
        LastKnot = "";

        for (int i = 0; i < choiceContainer.childCount; i++)
        {
            choiceButtons.Add(rootVisualElement.Q<Button>("Choice" + (i + 1).ToString()));
            choiceButtons[i].style.display = DisplayStyle.None;
            // directly passing i to delegate doesn't work because "Integer handled as reference type when passed into a delegate"
            // this means, the final value of i will be delegated to all the buttons (5 in this case)
            RegisterButtonOnlickEvents(i);
        }

        isPlaying = false;
        dialoguePanel.SetActive(false);
        mapPanel.SetActive(false);
        continueButton.clicked += ContinueStory;

        closeButton.clicked += OnClickClose;
        closeButtonMap.clicked += OnClickCloseMap;

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
        if(!isPlaying)
        {
            return;
        }


    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    void OnClickClose()
    {
        /*        Destroy(panel) ;*/
        dialoguePanel.SetActive(false);
        isPlaying = false;
        AvatarAnimator.Play("idle");
/*        ControllerManager.GetInstance().ExitUI();*/
    }

    void OnClickCloseMap()
    {
        mapPanel.SetActive(false);
        AvatarAnimator.Play("idle");
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
        AvatarAnimator.Play("talking");
        /*        guideAvatar.transform.LookAt(playerCamera.transform);*/
        ReForward(playerCamera.transform, guideAvatar);
        ReForward(playerCamera.transform, panelContainer);
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
        

        if (LastKnot.Equals("map2"))
        {
            mapPanel.SetActive(true);
        }
        else
        {
            AvatarAnimator.Play("idle");
/*            ControllerManager.GetInstance().ExitUI();*/
        }
    }

    public void ContinueStory()
    {
        
/*        Debug.Log(currStory.state.currentPointer);
        Debug.Log(currStory.state.currentTags);
        Debug.Log(currStory.state.currentFlowName);*/
        if (currStory.canContinue)
        {

            LastKnot = currStory.state.currentPathString.Split(".")[0];
/*            Debug.Log(LastKnot);*/
            dialogueLabel.text = currStory.Continue();
            continueButton.style.display = DisplayStyle.Flex;
            ShowChoices();
        }
        else
        {
            ExitDialogue();
        }
    }

    private void ShowChoices()
    {
        List<Choice> choices = currStory.currentChoices;

        if (choices.Count > 0)
        {
            continueButton.style.display = DisplayStyle.None;
        }

        if (choices.Count > choiceButtons.Count)
        {
            Debug.LogError("There are more choices than choice buttons.");
        }

        for (int i = 0;  i < choices.Count; i++)
        {
            choiceButtons[i].style.display = DisplayStyle.Flex;
            choiceButtons[i].text = choices[i].text;
        }

        for (int i = choices.Count; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].style.display = DisplayStyle.None;
        }


    }

    private void RegisterButtonOnlickEvents(int index)
    {
        choiceButtons[index].clicked += delegate { OnClickChoiceButton(index); };
    }

    void OnClickChoiceButton(int index)
    {
/*        Debug.Log(index);*/
        currStory.ChooseChoiceIndex(index);
        ContinueStory();
    }



}
