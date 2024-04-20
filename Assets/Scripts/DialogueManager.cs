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
    private Button continueButton;
    private VisualElement choiceContainer;
    private List<Button> choiceButtons;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Singleton class - more than one created");
        }
        instance = this;

        dialoguePanel = dialogueView.transform.Find("World Content").transform.Find("Flex Mesh").gameObject;
        rootVisualElement = (VisualElement)dialogueView.GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;
        dialogueLabel = rootVisualElement.Q<Label>("DialogueLabel");
        continueButton = rootVisualElement.Q<Button>("ContinueButton");
        choiceContainer = rootVisualElement.Q<VisualElement>("ScrollChoices");
        choiceButtons = new List<Button>();

        for (int i = 0; i < choiceContainer.childCount; i++)
        {
            choiceButtons.Add(rootVisualElement.Q<Button>("Choice" + (i + 1).ToString()));
            choiceButtons[i].style.display = DisplayStyle.None;
        
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
        continueButton.clicked += ContinueStory;
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
}
