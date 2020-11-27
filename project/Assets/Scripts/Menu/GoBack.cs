using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBack : ControllerInput
{
    private bool goingBack = false;

    void Update()
    {
        if (!goingBack && GetBackButton())
        {
            goingBack = true;
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        }
    }
}
