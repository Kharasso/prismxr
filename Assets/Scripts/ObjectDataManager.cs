using FlexXR.Runtime.FlexXRPanel;
using Keyboard;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static SaveData;

public class ObjectDataManager : MonoBehaviour, ISaveable
{   
    // new comment
    List<SaveData.CommentData> existingGuestComments;
    SaveData.CommentData existingCuratorComment = new SaveData.CommentData();
    SaveData.CommentData newGuestComment;
    
    string caller;
    public TextField guestNameField;
    public TextField guestCommentField;
    KeyboardManager keyboardManager;

    public VisualElement rootVisualElement;
    public GameObject keyboard;
    public GameObject panel;
    string focusedField;

    // Start is called before the first frame update
    void Start()
    {
        caller = this.transform.parent.name;
        if (rootVisualElement == null ) 
        {
            rootVisualElement = (VisualElement) GameObject.Find(caller).GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;
        }

        Button submitButton = rootVisualElement.Q<Button>("SubmitButton");
        Button closeButton = rootVisualElement.Q<Button>("CloseButton");
        submitButton.clicked += OnClick;
        closeButton.clicked += OnClickClose;

        guestNameField = rootVisualElement.Q<TextField>("NewGuestName");
        guestCommentField = rootVisualElement.Q<TextField>("NewGuestComment");

        keyboardManager = keyboard.GetComponent<KeyboardManager>();
        focusedField = "";

        HideKeyboard();
        LoadJsonData(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!keyboard.activeSelf && (guestNameField.panel.focusController.focusedElement == guestNameField || guestCommentField.panel.focusController.focusedElement == guestCommentField))
        {
            Debug.Log("keyboard");
            ShowKeyboard();

            Debug.Log(guestNameField.panel.focusController.focusedElement);

            if (guestNameField.panel.focusController.focusedElement == guestNameField)
            {
                focusedField = "name";
            }
            else if (guestCommentField.panel.focusController.focusedElement == guestCommentField)
            {
                focusedField = "comment";
            }
        }

        if(keyboard.activeSelf && rootVisualElement.Q<Foldout>("CommentToggle").value == false)
        {
            HideKeyboard() ;
        }

        if (keyboard.activeSelf && (guestNameField.panel.focusController.focusedElement == guestNameField || guestCommentField.panel.focusController.focusedElement == guestCommentField))
        {
            if (guestNameField.panel.focusController.focusedElement == guestNameField)
            {
               if (focusedField != "name")
                {
                    keyboardManager.OverwriteOutputField(guestNameField.value);
                }

               focusedField = "name";
            }
            else if (guestCommentField.panel.focusController.focusedElement == guestCommentField)
            {
                if (focusedField != "comment")
                {
                    keyboardManager.OverwriteOutputField(guestCommentField.value);
                }

                focusedField = "comment";
            }
        }

        switch (focusedField)
        {
            case "name":
                guestNameField.value = keyboardManager.GetOutputFieldText();
                break;
            case "comment":
                guestCommentField.value = keyboardManager.GetOutputFieldText();
                break;
            default:
                break;

        }
      

    }

    void OnClick()
    {
        SaveJsonData(this);
        focusedField = "";
    }

    void OnClickClose()
    {
        ControllerManager.GetInstance().ExitUI();
        Destroy(panel) ;
    }

    public void ShowKeyboard()
    {
        keyboard.SetActive(true);
        keyboardManager.ResetOutputField();
    }

    public void HideKeyboard()
    {
        keyboard.SetActive(false);
    }

    public void SaveJsonData(ObjectDataManager a_Saveables)
    {

        SaveData sd = new SaveData();

        a_Saveables.PopulateSaveData(sd);

        if (FileManager.WriteToFile(this.caller + "annotationData.dat", sd.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public void LoadJsonData(ObjectDataManager a_Saveables)
    {

        if (FileManager.LoadFromFile(this.caller + "annotationData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            a_Saveables.LoadFromSaveData(sd);

            Debug.Log("Load complete");
        }
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        TextField newGuestNameField = rootVisualElement.Q<TextField>("NewGuestName");
        TextField newGuestCommentField = rootVisualElement.Q<TextField>("NewGuestComment");

        if (newGuestCommentField.value.Length == 0 || newGuestNameField.value.Length == 0 || newGuestCommentField.value.Equals(" ") || newGuestNameField.value.Equals(" "))
        {
            Debug.Log("Guest Comment or Guest Name is empty!");
            /*            newGuestNameField.value = "dummy";
                        newGuestCommentField.value = "dummy";*/
            return;
        }


        newGuestComment.playerName = newGuestNameField.value;
        newGuestComment.playerCommentID = 1;
        newGuestComment.playerID = 1;
        newGuestComment.comment = newGuestCommentField.value;

        a_SaveData.m_curatorComment = existingCuratorComment;
        a_SaveData.m_guestComments = existingGuestComments;
        a_SaveData.m_guestComments.Add(newGuestComment);

        // display in the existing panel as well
        Label commentTextLabel = rootVisualElement.Q<Label>("GuestCommentText");

        if (commentTextLabel.text.Length > 0)
        {
            commentTextLabel.text = Environment.NewLine + Environment.NewLine + commentTextLabel.text;
        }
        commentTextLabel.text = newGuestComment.playerName + ":" + Environment.NewLine + newGuestComment.comment + commentTextLabel.text;
        newGuestCommentField.value = " ";
        newGuestNameField.value = " ";
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {
        // curator data
        if (object.Equals(existingCuratorComment, default(SaveData.CommentData)))
        {
            existingCuratorComment = a_SaveData.m_curatorComment;
            rootVisualElement.Q<Label>("CuratorComment").text = a_SaveData.m_curatorComment.comment;
        }

        // guest data
        existingGuestComments = new List<SaveData.CommentData>();
        Label commentTextLabel = rootVisualElement.Q<Label>("GuestCommentText");

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


