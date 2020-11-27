using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseVolume : MonoBehaviour
{
    public UnityEngine.Audio.AudioMixer mixer;

    public void Increase(bool inc)
    {
        float val;
        mixer.GetFloat("masterVol", out val);

        mixer.SetFloat("masterVol", Mathf.Clamp(val + (inc ? -1f : 1f), -80f, 20f));
    }
}
