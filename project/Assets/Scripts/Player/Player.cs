using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : ControllerInput
{
    private Vector3 moveDirection = Vector3.zero;
    private float xPos = 0f;
    private float yPos = 0f;
    private int controllerCache = 0;

    private CharacterController controller;

    [SerializeField] private float moveSpeed = 6.0F;
    [SerializeField] private float gravity = 6.0F;
    [SerializeField] private Camera followCam;

    public override void Start()
    {
        base.Start();

        controller = GetComponent<CharacterController>();
    }

    private void Move()
    {
        if (!controllerExists) return;

        xPos += GetHorizontalAxis();
        yPos += GetVerticalAxis();

        //transform.Rotate(0, xPos, 0);

        moveDirection = transform.forward;
        moveDirection *= yPos;

        Vector3 right = transform.right;
        right *= xPos;

        moveDirection = right + moveDirection;

        moveDirection.y -= Time.deltaTime * gravity;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        xPos = 0;
        yPos = 0;
    }

    void Update()
    {
        Move();

        if (controllerCache != ControllerInput.available)
        {
            controllerCache = ControllerInput.available;
            UpdateCameraPosition(followCam, ControllerInput.available);
        }
    }
}
