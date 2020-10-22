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
    [SerializeField] private float turnSpeed = 6.0F;
    [SerializeField] private float gravity = 6.0F;
    [SerializeField] private Camera followCam;
    [SerializeField] private Transform character;

    public override void Start()
    {
        base.Start();

        if (PlayerManager.settings.shouldOverride)
        {
            moveSpeed = PlayerManager.settings.globalMoveSpeed;
            turnSpeed = PlayerManager.settings.globalTurnSpeed;
            gravity = PlayerManager.settings.globalGravity;
        }

        startPos = transform.position;
        controller = GetComponent<CharacterController>();

        if (!controllerExists) gameObject.SetActive(false);
    }

    private void Move()
    {
        if (!controllerExists) return;

        xPos += GetHorizontalAxis();
        yPos += GetVerticalAxis();

        //transform.Rotate(0, xPos, 0);

        moveDirection = followCam.transform.forward;
        moveDirection.y = 0;
        moveDirection *= yPos;

        Vector3 right = followCam.transform.right;
        right *= xPos;

        moveDirection = right + moveDirection;

        moveDirection.y -= Time.deltaTime * gravity * 10;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        followCam.transform.LookAt(transform);
        followCam.transform.RotateAround(transform.position, new Vector3(0.0f, 1.0f, 0.0f), turnSpeed * GetRHorizontalAxis());

        Vector3 rot = followCam.transform.eulerAngles;
        rot.x = 0;
        rot.y += 90;
        rot.z = 0;

        character.eulerAngles = rot;
        //followCam.transform.Translate(Vector3.right * GetRHorizontalAxis());

        xPos = 0;
        yPos = 0;
    }

    void FixedUpdate()
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
