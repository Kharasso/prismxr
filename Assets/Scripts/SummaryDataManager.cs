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

public class SummaryDataManager : MonoBehaviour, ISaveable
{   
    // new comment
    List<SaveData.CommentData> existingGuestComments;
    SaveData.CommentData existingCuratorComment = new SaveData.CommentData();
    
    string caller;

    public VisualElement rootVisualElement;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        caller = this.transform.parent.name;
        if (rootVisualElement == null ) 
        {
            rootVisualElement = (VisualElement) GameObject.Find(caller).GetComponent<FlexXRPanelManager>().flexXRPanelElements.FlexXRContentElement;
        }

        Button closeButton = rootVisualElement.Q<Button>("CloseButton");
        closeButton.clicked += OnClickClose;

        LoadJsonData(this);
    }

    // Update is called once per frame
    void Update()
    {
     
      

    }

    void OnClickClose()
    {
        Destroy(panel) ;
    }


    public void SaveJsonData(SummaryDataManager a_Saveables)
    {

        SaveData sd = new SaveData();

        a_Saveables.PopulateSaveData(sd);

        if (FileManager.WriteToFile(this.caller + "annotationData.dat", sd.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public void LoadJsonData(SummaryDataManager a_Saveables)
    {

        List<string> artNums = new List<string>();
        artNums.Add("art1");
        artNums.Add("art2");
        artNums.Add("art3");

        foreach (string num in artNums)
        {
            if (FileManager.LoadFromFile("AnnotationUI" + num + "annotationData.dat", out var json))
            {
                SaveData sd = new SaveData();
                sd.LoadFromJson(json);

                a_Saveables.LoadArt(sd, num);

                Debug.Log("Load complete");
            }
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

        a_SaveData.m_curatorComment = existingCuratorComment;
        a_SaveData.m_guestComments = existingGuestComments;

        // display in the existing panel as well
        Label commentTextLabel = rootVisualElement.Q<Label>("GuestCommentText");

        if (commentTextLabel.text.Length > 0)
        {
            commentTextLabel.text = Environment.NewLine + Environment.NewLine + commentTextLabel.text;
        }

        newGuestCommentField.value = " ";
        newGuestNameField.value = " ";
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {

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


