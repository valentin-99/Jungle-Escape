using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VolumeData
{
    public float valueMusic;
    public float valueSFX;

    public VolumeData(MenuInteraction settings)
    {
        valueMusic = settings.valueMusic;
        valueSFX = settings.valueSFX;
    }
}
