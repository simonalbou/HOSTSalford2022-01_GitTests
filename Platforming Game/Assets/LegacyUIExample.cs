using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyUIExample : MonoBehaviour
{
    // IMGUI
    void OnGUI()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Hello world!"))
        {
            Debug.Log("Hello world!");
        }

        GUILayout.EndHorizontal();
    }
}
