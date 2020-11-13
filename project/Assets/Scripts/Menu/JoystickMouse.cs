using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMouse : ControllerInput
{
    private Rect canvas;

    [Header("Controls")]
    [SerializeField] private float cursorPosX = 600f;
    [SerializeField] private float cursorPosY = 800f;

    [SerializeField] private float mouseSpeed = 1f;

    public override void Start()
    {
        base.Start();

        canvas = GetComponentInParent<Canvas>().pixelRect;
    }

    private void Move()
    {
        if (!controllerExists) return;

        cursorPosX += GetHorizontalAxis() * mouseSpeed;
        cursorPosY += GetVerticalAxis() * mouseSpeed;
        
        if (cursorPosX >= canvas.width) cursorPosX = canvas.width; else if (cursorPosX <= 0) cursorPosX = 0;
        if (cursorPosY >= canvas.height) cursorPosY = canvas.height; else if (cursorPosY <= 0) cursorPosY = 0;

        //transform.Rotate(0, xPos, 0);
        transform.position = new Vector3(cursorPosX, cursorPosY, 0);
    }

    void FixedUpdate()
    {
        Move();
    }
}
