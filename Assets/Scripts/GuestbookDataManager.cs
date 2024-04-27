using FlexXR.Runtime.FlexXRPanel;
using Keyboard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static SaveData;

public class GuestbookDataManager : MonoBehaviour, ISaveable
{   
    
    string caller;

    public GameObject panel;
    public GameObject keyboard;
    KeyboardManager keyboardManager;

    public VisualElement rootVisualElement;
    public VisualElement leftPageContainer;
    public VisualElement rightPageContainer;
    public VisualElement modalContainer;
    public VisualElement coverPageContainer;

    List<VisualElement> allItemCells = new List<VisualElement>();
    List<SaveData.CommentData> existingGuestComments = new List<SaveData.CommentData>();
    int activeCell = 0;
    Label rightPageGuestName;
    Label rightPageCommentDate;
    Label rightPageCommentText;

    TextField newGuestNameField;
    TextField newGuestCommentField;

    string focusedField;

    // Start is called before the first frame update
    void Start()
    {
        caller = this.transform.parent.name;
        if (rootVisualElement == null ) 
        {
            rootVisualElement = (VisualElement) GameObject.Find(caller).GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;
        }

        Debug.Log(rootVisualElement.name);
        keyboardManager = keyboard.GetComponent<KeyboardManager>();

        leftPageContainer = rootVisualElement.Q<VisualElement>("LeftPageContainer");
        rightPageContainer = rootVisualElement.Q<VisualElement>("RightPageContainer");
        modalContainer = rootVisualElement.Q<VisualElement>("ModalContainer");
        coverPageContainer = rootVisualElement.Q<VisualElement>("CoverPageContainer");

        rightPageGuestName = rightPageContainer.Q<Label>("GuestName");
        rightPageCommentDate = rightPageContainer.Q<Label>("CommentDate");
        rightPageCommentText = rightPageContainer.Q<Label>("CommentText");

        newGuestNameField = modalContainer.Q<TextField>("NewGuestName");
        newGuestCommentField = modalContainer.Q<TextField>("NewGuestComment");


        Button closeButtonCover = rootVisualElement.Q<Button>("CloseButtonCover");
        Button closeButtonPage = rootVisualElement.Q<Button>("CloseButtonPage");
        Button openButton = coverPageContainer.Q<Button>("OpenButton");
        Button openCommentButton = leftPageContainer.Q<Button>("OpenCommentButton");
        Button closeCommentButton = modalContainer.Q<Button>("CloseModalButton");
        Button submitCommentButton = modalContainer.Q<Button>("SubmitCommentButton");

        closeButtonCover.clicked += OnClickClose;
        closeButtonPage.clicked += OnClickClose;
        openButton.clicked += OnClickOpen;
        openCommentButton.clicked += OnClickOpenModal;
        closeCommentButton.clicked += OnClickCloseModal;
        submitCommentButton.clicked += OnClickSubmitModal;

        coverPageContainer.style.display = DisplayStyle.Flex;
        leftPageContainer.style.display = DisplayStyle.None;
        rightPageContainer.style.display = DisplayStyle.None;
        modalContainer.style.display = DisplayStyle.None;

        focusedField = "";

        HideKeyboard();
        LoadJsonData(this);
        renderContentRightPage();
    }

    // Update is called once per frame
    void Update()
    {
        if (!keyboard.activeSelf && (newGuestNameField.panel.focusController.focusedElement == newGuestNameField || newGuestCommentField.panel.focusController.focusedElement == newGuestCommentField))
        {
            Debug.Log("keyboard");
            ShowKeyboard();

            Debug.Log(newGuestNameField.panel.focusController.focusedElement);

            if (newGuestNameField.panel.focusController.focusedElement == newGuestNameField)
            {
                focusedField = "name";
            }
            else if (newGuestCommentField.panel.focusController.focusedElement ==newGuestCommentField)
            {
                focusedField = "comment";
            }
        }


        if (keyboard.activeSelf && (newGuestNameField.panel.focusController.focusedElement == newGuestNameField || newGuestCommentField.panel.focusController.focusedElement == newGuestCommentField))
        {
            if (newGuestNameField.panel.focusController.focusedElement == newGuestNameField)
            {
                if (focusedField != "name")
                {
                    keyboardManager.OverwriteOutputField(newGuestNameField.value);
                }

                focusedField = "name";
            }
            else if (newGuestCommentField.panel.focusController.focusedElement == newGuestCommentField)
            {
                if (focusedField != "comment")
                {
                    keyboardManager.OverwriteOutputField(newGuestCommentField.value);
                }

                focusedField = "comment";
            }
        }

        switch (focusedField)
        {
            case "name":
                newGuestNameField.value = keyboardManager.GetOutputFieldText();
                break;
            case "comment":
                newGuestCommentField.value = keyboardManager.GetOutputFieldText();
                break;
            default:
                break;

        }


    }

    void OnClickClose()
    {
/*        Destroy(panel) ;*/
        panel.SetActive(false);
        ControllerManager.GetInstance().ExitUI();
    }

    // open guestbook
    void OnClickOpen()
    {
        coverPageContainer.style.display = DisplayStyle.None;
        leftPageContainer.style.display = DisplayStyle.Flex;
        rightPageContainer.style.display = DisplayStyle.Flex;
    }

    // open comment modal
    void OnClickOpenModal()
    {
        modalContainer.style.display = DisplayStyle.Flex;
    }

    // close comment modal
    void OnClickCloseModal()
    {
        focusedField = "";
        newGuestCommentField.value = " ";
        newGuestNameField.value = " ";
        HideKeyboard();
        modalContainer.style.display= DisplayStyle.None;
    }

    // submit comment modal
    void OnClickSubmitModal()
    {
        SaveJsonData(this);
        OnClickCloseModal();
    }

    // hide and show keyword
    public void ShowKeyboard()
    {
        keyboard.SetActive(true);
        keyboardManager.ResetOutputField();
    }

    public void HideKeyboard()
    {
        keyboard.SetActive(false);
    }

    // create new content row
    VisualElement CreateNewContentRow(VisualElement targetParent)
    {
        VisualElement newRow = new VisualElement();
        newRow.AddToClassList("guestbook-content-row");
        targetParent.Add(newRow);

        return newRow;
    }

    VisualElement CreateNewContentCell(VisualElement targetParent, int order, bool empty = false) {

        VisualElement newCell = new VisualElement();
        newCell.AddToClassList("guestbook-content-cell");

        // if empty, add the empty content and style to the cell
        if (empty)
        {
            VisualElement emptyContent = new VisualElement();
            emptyContent.AddToClassList("guestbook-content-empty");

            Label textEmpty = new Label();
            textEmpty.AddToClassList("guestbook-content-text-empty");

            textEmpty.text = "+";

            emptyContent.Add(textEmpty);
            newCell.Add(emptyContent);
        }
        // if not empty, add the content cell
        else
        {
            Button button = new Button();
            button.AddToClassList("guestbook-content-button");
            button.clicked += delegate { renderContentRightPage(order);  };

            Label textBig = new Label();
            textBig.AddToClassList("guestbook-content-text-big");
            textBig.name = "LabelBig";
            textBig.style.color = new StyleColor(new Color(253, 253, 253, 255));

            Label textSmall = new Label();
            textSmall.AddToClassList("guestbook-content-text-small");
            textSmall.name = "LabelSmall";
            textSmall.style.color = new StyleColor(new Color(253, 253, 253, 255));

            button.Add(textBig);
            button.Add(textSmall);

            newCell.Add(button);
        }
        

        targetParent.Add(newCell);

        return newCell;
    }


    public void SaveJsonData(GuestbookDataManager a_Saveables)
    {

        SaveData sd = new SaveData();

        a_Saveables.PopulateSaveData(sd);

        if (FileManager.WriteToFile("guestbookData.dat", sd.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public void LoadJsonData(GuestbookDataManager a_Saveables)
    {

      
        FileManager.LoadFromFile("guestbookData.dat", out var json);
            
        SaveData sd = new SaveData();
        sd.LoadFromJson(json);
        a_Saveables.LoadFromSaveData(sd);

        Debug.Log("Load complete");

        
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {

        if (newGuestCommentField.value.Length == 0 || newGuestNameField.value.Length == 0 || newGuestCommentField.value.Equals(" ") || newGuestNameField.value.Equals(" "))
        {
            Debug.Log("Guest Comment or Guest Name is empty!");
            /*            newGuestNameField.value = "dummy";
                        newGuestCommentField.value = "dummy";*/
            return;
        }

        SaveData.CommentData newGuestComment = new SaveData.CommentData();

        newGuestComment.playerName = newGuestNameField.value;
        newGuestComment.playerCommentID = 1;
        newGuestComment.playerID = 1;
        newGuestComment.comment = newGuestCommentField.value;
        newGuestComment.commentEpoch = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        existingGuestComments.Add(newGuestComment);
        a_SaveData.m_guestComments = existingGuestComments;
        activeCell = 0;

        // display in the existing panel as well
        renderContentCells();
        renderContentRightPage();
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {

        // make sure the item decks are empty
        existingGuestComments = new List<SaveData.CommentData>();

        for (int i = 0; i < a_SaveData.m_guestComments.Count; i++)
        {
            existingGuestComments.Add(a_SaveData.m_guestComments.ElementAt(i)); 
        }

        Debug.Log(existingGuestComments.Count);
        renderContentCells();
        activeCell = 0;

        /*       commentTextLabel.text = "";

               Debug.Log(a_SaveData.m_guestComments.Count);

               foreach (SaveData.CommentData commentData in a_SaveData.m_guestComments)
               {
                   existingGuestComments.Add(commentData);

                   if (commentTextLabel.text.Length > 0)
                   {
                       commentTextLabel.text = Environment.NewLine + Environment.NewLine + commentTextLabel.text;
                   }

                   commentTextLabel.text = commentData.playerName + ":" + Environment.NewLine + commentData.comment + commentTextLabel.text;

                   Debug.Log(commentData.comment);
               }*/
    }

    void renderContentCells()
    {
        // empty the item cell deck
        allItemCells = new List<VisualElement>();
        int numItems = existingGuestComments.Count;
        int fullRows = numItems / 3;
        int remainder = numItems % 3;

        // select the unity content container and make sure it is empty
        VisualElement contenArea = leftPageContainer.Q<VisualElement>("ScrollView").Q<VisualElement>("unity-content-container");
        contenArea.Clear();

        for (int i = 0; i < fullRows; i++)
        {
            VisualElement newRow = CreateNewContentRow(contenArea);

            for (int j = 0; j < 3; j++)
            {
                VisualElement cell = CreateNewContentCell(newRow, i*3+j);
                allItemCells.Add(cell);
           
            }

        }

        // add the incomplete row, if required
        if (remainder > 0)
        {
            VisualElement newRow = CreateNewContentRow(contenArea);

            for (int j = 0; j < 3; j++)
            {
                if (remainder > j)
                {
                    VisualElement cell = CreateNewContentCell(newRow, fullRows*3+j);
                    allItemCells.Add(cell);
                }
                else
                {
                    CreateNewContentCell(newRow, fullRows * 3 + j, true);
                }
            }
        }

        // populate the items in reverse order
        for (int i = 0; i < numItems; i++)
        {
            allItemCells.ElementAt(numItems - i - 1).Q<Label>("LabelBig").text = existingGuestComments.ElementAt(i).playerName;

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(existingGuestComments.ElementAt(i).commentEpoch);
            allItemCells.ElementAt(numItems - i - 1).Q<Label>("LabelSmall").text = dateTimeOffset.DateTime.ToString("yyyy-MM-dd");
        }

    }

    void renderContentRightPage(int newActive = 0)
    {
        SaveData.CommentData currCommentData = existingGuestComments.ElementAt(existingGuestComments.Count - newActive - 1);
        rightPageGuestName.text = currCommentData.playerName;

        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(currCommentData.commentEpoch);
        rightPageCommentDate.text = dateTimeOffset.DateTime.ToString("yyyy-MM-dd");

        rightPageCommentText.text = currCommentData.comment;
    }

    public void LoadArt(SaveData a_SaveData, string artNum)
    {

        // guest data
        existingGuestComments = new List<SaveData.CommentData>();
        Label commentTextLabel = rootVisualElement.Q<Label>(artNum + "Comments");

        commentTextLabel.text = "";

        Debug.Log(a_SaveData.m_guestComments.Count);

        foreach (SaveData.CommentData commentData in a_SaveData.m_guestComments)
        {
            existingGuestComments.Add(commentData);

            if (commentTextLabel.text.Length > 0)
            {
                commentTextLabel.text = Environment.NewLine + Environment.NewLine + commentTextLabel.text;
            }

            commentTextLabel.text = commentData.playerName + ":" + Environment.NewLine + commentData.comment + commentTextLabel.text;

            Debug.Log(commentData.comment);
        }
    }

}


