using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct CommentData
    {
        public string comment;
        public string playerName;
        public long commentEpoch;
        public int playerID;
        public int playerCommentID;
    }
    public List<CommentData> m_guestComments = new List<CommentData>();
    public CommentData m_curatorComment = new CommentData();
    
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData a_SaveData);

    void LoadFromSaveData(SaveData a_SaveData);
}

