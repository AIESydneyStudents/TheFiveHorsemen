using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    static public int available = 0; // Will always be available anywhere.

    [HideInInspector] 
    public enum availableControllers
    {
        Controller1 = 1,
        Controller2,
        Controller3,
        Controller4
    };

    [Header("Player Configuration")]

    // The reason I serialized a public string is because it will put it in the editor even if its inherited.
    [SerializeField] private availableControllers selectedController = availableControllers.Controller1;

    public string controllerInput { get; private set; }
    public bool controllerExists { get; private set; }

    public virtual void Start()
    {
        controllerInput = selectedController.ToString();
        controllerExists = ControllerExists();

        if (controllerExists)
        {
            ControllerInput.available += 1;
        }

        Debug.Log(controllerInput + " : " + controllerExists);
    }

    public bool ControllerExists()
    {
        return (Input.GetJoystickNames().Length + 1) > (int)selectedController;
    }

    public float GetHorizontalAxis()
    {
        return (controllerExists) ? Input.GetAxis(controllerInput + "_Horizontal") : 0;
    }

    public float GetVerticalAxis()
    {
        return (controllerExists) ? Input.GetAxis(controllerInput + "_Vertical") : 0;
    }

    public float GetRightTrigger()
    {
        return (controllerExists) ? Input.GetAxis(controllerInput + "_R_Trigger") : 0;
    }

    public bool Clicking()
    {
        return controllerExists ? (GetRightTrigger() < 0) : true;
    }

    public void UpdateCameraPosition(Camera cam, int cams)
    {
        // Welcome to cancer!
        cam.rect = ControllerInputConfig.splitScreenController[cams - 2].splits[(int)selectedController - 1].viewPort;
    }
}
