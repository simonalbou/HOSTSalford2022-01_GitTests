using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioTests : MonoBehaviour
{
    public AudioMixer myMixer;

    // We exposed a parameter to script from the Mixer window, and called it "_VolumeOfEnvironmentFX" ourselves
    void Start()
    {
        // The "out" keyword allows storing a result into a parameter.
        // This is common for functions that need to output multiple things, where there can only be one single return value. 

        // method 1 : declare the variable then use it to hold the result
        float result = 0f;
        myMixer.GetFloat("_VolumeOfEnvironmentFX", out result);
        
        // method 2 (inline declaration) : declare the variable within the function call
        //myMixer.GetFloat("_VolumeOfEnvironmentFX", out float result);
        
        Debug.Log(result); // prints the current value of our parameter

        // sets the value to whatever we want
        myMixer.SetFloat("_VolumeOfEnvironmentFX", 800f);

        // will still return the same thing as previously,
        // because there's no reason a link would exist between this variable and the mixer
        Debug.Log(result);
    }
}
