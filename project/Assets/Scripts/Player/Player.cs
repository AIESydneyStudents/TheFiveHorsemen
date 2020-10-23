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

    private float pushCooldown = 0;
    private bool jumping = false;

    [SerializeField] private float moveSpeed = 6.0F;
    [SerializeField] private float turnSpeed = 6.0F;
    [SerializeField] private float gravity = 6.0F;
    [SerializeField] private float jumpHeight = 10.0F;
    [SerializeField] private Camera followCam;
    [SerializeField] private Transform cameraAnchor;
    [SerializeField] private Transform character;

    public override void Start()
    {
        base.Start();

        if (PlayerManager.settings.shouldOverride)
        {
            moveSpeed = PlayerManager.settings.globalMoveSpeed;
            turnSpeed = PlayerManager.settings.globalTurnSpeed;
            gravity = PlayerManager.settings.globalGravity;
            jumpHeight = PlayerManager.settings.globalJumpHeight;
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

        moveDirection = new Vector3(followCam.transform.forward.x * yPos, jumping ? moveDirection.y : 0, followCam.transform.forward.z * yPos);

        Vector3 right = followCam.transform.right;
        right *= xPos;

        moveDirection = right + moveDirection;

        if (GetJumpButton() && controller.isGrounded && !jumping)
        {
            jumping = true;
            moveDirection.y = jumpHeight;
        }
        else if (jumping && controller.isGrounded)
        {
            moveDirection.y = 0;
            jumping = false;
        }
        
        moveDirection.y -= Time.deltaTime * gravity * (jumping ? 1 : 10);

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        followCam.transform.LookAt(cameraAnchor);
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

    void Push()
    {
        RaycastHit hit;

        int layerMask = 1 << 9;

        Vector3 dir = followCam.transform.forward;
        dir.y = 0;

        if (Physics.Raycast(transform.position, dir, out hit, 0.5f, layerMask))
        {
            CharacterController rb;

            if (hit.transform.TryGetComponent(out rb))
            {
                rb.Move(dir * 2f);
            }

            pushCooldown = 1;
        }
    }

    void FixedUpdate()
    {
        Move();

        if (pushCooldown > 0) pushCooldown -= Time.deltaTime; 

        if (Clicking() && pushCooldown <= 0) Push();

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
