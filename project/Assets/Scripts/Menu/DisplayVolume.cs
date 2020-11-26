using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayVolume : MonoBehaviour
{
    public UnityEngine.Audio.AudioMixer mixer;
    public UnityEngine.UI.Text text;

    // Update is called once per frame
    void Update()
    {
        float val;
        mixer.GetFloat("masterVol", out val);

        text.text = 100 + val + "%";
    }
}
