using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerToText : MonoBehaviour
{
    public UnityEngine.UI.Text txt;

    void Update()
    {
        txt.text = "Player" + (int)PlayerManager.winner;
    }
}
