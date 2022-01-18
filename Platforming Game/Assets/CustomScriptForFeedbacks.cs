using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MyCustomStruct
{
    public float someFloatParam;
    public Color someColor;
}

public class CustomScriptForFeedbacks : MonoBehaviour
{
    public Color someNewColor;
    public SpriteRenderer selfRenderer;

    public MyCustomStruct myBundledValues;

    void Start()
    {
        Debug.Log(myBundledValues.someFloatParam);
    }

    public void ChangeColorAndPrintSomething(string consoleMsg)
    {
        selfRenderer.color = someNewColor;

        Debug.Log(consoleMsg);
    }

    public void DoABunchOfThings(Transform tr, Color col, string str)
    {
        Debug.Log(tr.name);
        selfRenderer.color = col;
        Debug.Log(str);
    }
}
