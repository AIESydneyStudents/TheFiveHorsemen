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

    public bool ControllerExists() // Jank
    {
        bool exists = false;
        int select = (int)selectedController - 1;

        if (Input.GetJoystickNames().Length > select && Input.GetJoystickNames()[select].Length > 0)
        {
            exists = true;
        }

        return PlayerManager.settings.debug || exists;
    }

    public float GetHorizontalAxis()
    {
        return (controllerExists) ? Input.GetAxis(controllerInput + "_Horizontal") : 0;
    }

    public float GetRHorizontalAxis()
    {
        return (controllerExists) ? Input.GetAxis(controllerInput + "_R_Horizontal") : 0;
    }

    public float GetVerticalAxis()
    {
        return (controllerExists) ? Input.GetAxis(controllerInput + "_Vertical") : 0;
    }

    public float GetRVerticalAxis()
    {
        return (controllerExists) ? Input.GetAxis(controllerInput + "_R_Vertical") : 0;
    }

    public float GetRightTrigger()
    {
        return (controllerExists) ? Input.GetAxis(controllerInput + "_R_Trigger") : 0;
    }

    public bool Clicking()
    {
        return controllerExists ? (GetRightTrigger() < 0) : true;
    }

    public bool GetJumpButton()
    {
        return (controllerExists) ? Input.GetButton(controllerInput + "_Jump") : false;
    }

    public bool GetBackButton()
    {
        return (controllerExists) ? Input.GetButton(controllerInput + "_Back") : false;
    }

    public void UpdateCameraPosition(Camera cam, int cams)
    {
        // Welcome to cancer!
        int index = cams;

        switch (cams)
        {
            case 1:
                index = -1;
                break;
            case 2:
                index = 0;
                break;
            case 3:
                index = 1;
                break;
            case 4:
                index = 2;
                break;
        }

        cam.rect = ControllerInputConfig.splitScreenController[index].splits[(int)selectedController - 1].viewPort;
    }
}
