using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTests : MonoBehaviour
{
    public ParticleSystem selfSystem;

    // Let's call this in the player jump event (hence the function's name)
    public void OnPlayerJump()
    {
        // 1) access the module by storing it into a dedicated variable
        ParticleSystem.MainModule mainModule = selfSystem.main;

        // 2) access one single parameter of the module by storing it into a dedicated variable
        // ParticleSystem don't use Colors and floats, but MinMaxGradients and MinMaxFloats
        ParticleSystem.MinMaxGradient newColor = mainModule.startColor;
        newColor.mode = ParticleSystemGradientMode.Color;
        newColor.color = Color.blue;
        // 3) assign the value back in its module
        mainModule.startColor = newColor;

        // 4) assign the module back to its component? Well, no!
        // Due to how ParticleSystems are coded, this is not required and it wouldn't work anyway.
        //selfSystem.main = mainModule;

        // And then, finally, we just play our system.
        selfSystem.Play();
    }
}
