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
    private Vector3 startPos;

    [SerializeField] private float moveSpeed = 6.0F;
    [SerializeField] private float gravity = 6.0F;
    [SerializeField] private Camera followCam;

    public override void Start()
    {
        base.Start();
        startPos = transform.position;
        controller = GetComponent<CharacterController>();
    }

    private void Move()
    {
        if (!controllerExists) return;

        xPos += GetHorizontalAxis();
        yPos += GetVerticalAxis();

        //transform.Rotate(0, xPos, 0);

        moveDirection = followCam.transform.forward;
        moveDirection *= yPos;

        Vector3 right = followCam.transform.right;
        right *= xPos;

        moveDirection = right + moveDirection;

        moveDirection.y -= Time.deltaTime * gravity * 100;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        followCam.transform.LookAt(transform);
        followCam.transform.RotateAround(transform.position, new Vector3(0.0f, 1.0f, 0.0f), 3 * GetRHorizontalAxis());
        //followCam.transform.Translate(Vector3.right * GetRHorizontalAxis());

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

        if (transform.position.y <= -3.3)
        {
            transform.position = startPos;
        }
    }
}
