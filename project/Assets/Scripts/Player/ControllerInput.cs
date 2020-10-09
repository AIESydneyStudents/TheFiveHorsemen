using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    enum availableControllers
    {
        Controller1 = 1,
        Controller2,
        Controller3,
        Controller4
    };

    // The reason I serialized a public string is because it will put it in the editor even if its inherited.
    [SerializeField] private availableControllers selectedController = availableControllers.Controller1;

    public string controllerInput { get; private set; }
    public bool controllerExists { get; private set; }

    public virtual void Start()
    {
        controllerInput = selectedController.ToString();
        controllerExists = ControllerExists();

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
}
