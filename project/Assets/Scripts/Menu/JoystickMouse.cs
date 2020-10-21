using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMouse : ControllerInput
{
    private float xPos = 600f;
    private float yPos = 800f;

    [SerializeField] private float mouseSpeed = 1f;

    public override void Start()
    {
        base.Start();
    }

    private void Move()
    {
        if (!controllerExists) return;

        xPos += GetHorizontalAxis() * mouseSpeed;
        yPos += GetVerticalAxis() * mouseSpeed;

        if (xPos >= 1920) xPos = 1920; else if (xPos <= 0) xPos = 0;
        if (yPos >= 1080) yPos = 1080; else if (yPos <= 0) yPos = 0;

        //transform.Rotate(0, xPos, 0);
        transform.position = new Vector3(xPos, yPos, 0);
    }

    void Update()
    {
        Move();
    }
}
