using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static InitDataManager instance;
    public TextAsset art1;
    public TextAsset art2;
    public TextAsset art3;
    public TextAsset art4;
    public TextAsset art5;
    public TextAsset art6;
    public TextAsset art7;
    public TextAsset art8;
    public TextAsset art9;
    public TextAsset art10;
    public TextAsset art11;
    public TextAsset art12;
    public TextAsset art13;
    public TextAsset art14;
    public Dictionary<string, string> artData = new Dictionary<string, string>();

    public TextAsset summaryData;
    public TextAsset guestbookData;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Singleton class - more than one created");
        }
        instance = this;
    }

    void Start()
    {
        artData.Add("AnnotationUIart1", art1.ToString());
        artData.Add("AnnotationUIart2", art2.ToString());
        artData.Add("AnnotationUIart3", art3.ToString());
        artData.Add("AnnotationUIart4", art4.ToString());
        artData.Add("AnnotationUIart5", art5.ToString());
        artData.Add("AnnotationUIart6", art6.ToString());
        artData.Add("AnnotationUIart7", art7.ToString());
        artData.Add("AnnotationUIart8", art8.ToString());
        artData.Add("AnnotationUIart9", art9.ToString());
        artData.Add("AnnotationUIart10", art10.ToString());
        artData.Add("AnnotationUIart11", art11.ToString());
        artData.Add("AnnotationUIart12", art12.ToString());
        artData.Add("AnnotationUIart13", art13.ToString());
        artData.Add("AnnotationUIart14", art14.ToString());

    }

    public static InitDataManager GetInstance()
    {
        return instance;
    }

}
