using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayingWithUIAndTexts : MonoBehaviour
{
    public RectTransform buttonTransform;
    public Image myImage;
    public TextMeshProUGUI myText;

    public GameObject windowObject;

    void Start()
    {
        //myImage.enabled = false;
        myImage.color = Color.blue;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            myText.text = "<b>Hello</b> world!";
        }
    }

    public void DisappearWindow()
    {
        windowObject.SetActive(false);
    }
}
