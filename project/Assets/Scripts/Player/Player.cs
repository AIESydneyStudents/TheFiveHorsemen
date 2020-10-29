using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : ControllerInput
{
    #region Properties
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private bool dashing = false;

    private float xPos = 0f;
    private float yPos = 0f;
    private int controllerCache = 0;

    private CharacterController controller;
    private Vector3 startPos;

    private float pushCooldown = 0;
    private bool jumping = false;

    [SerializeField] private Camera followCam;
    [SerializeField] private Transform cameraAnchor;
    [SerializeField] private Transform character;
    #endregion

    public override void Start()
    {
        base.Start();

        startPos = transform.position;
        controller = GetComponent<CharacterController>();
    }

    #region Functionality
    /// <summary>
    /// Adding velocity to the player.
    /// </summary>
    /// <param name="vel">Vector3 directional velocity to add.</param>
    public void AddVelocity(Vector3 vel)
    {
        velocity += vel;
    }

    /// <summary>
    /// Moving function.
    /// </summary>
    private void Move()
    {
        float moveSpeed = PlayerManager.settings.globalMoveSpeed;
        float turnSpeed = PlayerManager.settings.globalTurnSpeed;
        float gravity = PlayerManager.settings.globalGravity;
        float jumpHeight = PlayerManager.settings.globalJumpHeight;
        float friction = PlayerManager.settings.globalFriction;

        xPos += GetHorizontalAxis();
        yPos += GetVerticalAxis();

        //transform.Rotate(0, xPos, 0);

        moveDirection = new Vector3(followCam.transform.forward.x * yPos + velocity.x, (jumping ? moveDirection.y : 0) + velocity.y, followCam.transform.forward.z * yPos + velocity.z);

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

        float mult = friction/100;

        if (velocity.x > 0.1 || velocity.x < -0.1)
            velocity.x = velocity.x > 0.1 ? velocity.x - mult : velocity.x + mult;
        else velocity.x = 0;

        if (velocity.z > 0.1 || velocity.z < -0.1)
            velocity.z = velocity.z > 0.1 ? velocity.z - mult : velocity.z + mult;
        else velocity.z = 0;

        xPos = 0;
        yPos = 0;
    }

    /// <summary>
    /// Pushing other players/objects function.
    /// </summary>
    void Push()
    {
        RaycastHit hit;

        int layerMask = 1 << 9;

        Vector3 dir = followCam.transform.forward;
        dir.y = 0;

        if (Physics.Raycast(transform.position, dir, out hit, 0.5f, layerMask))
        {
            Player rb;

            if (hit.transform.TryGetComponent(out rb))
            {
                rb.AddVelocity(dir * PlayerManager.settings.globalPushStrength);
            }
        }
    }

    /// <summary>
    /// Function runner.
    /// </summary>
    void FixedUpdate()
    {
        if (!PlayerManager.settings.debug && !controllerExists) gameObject.SetActive(false);

        Move();

        if (pushCooldown > 0) pushCooldown -= Time.deltaTime;

        if (dashing)
        {
            if (pushCooldown <= 0) dashing = false;

            Push();
        }

        if (!dashing && Clicking() && pushCooldown <= 0)
        {
            pushCooldown = 3;
            dashing = true;

            Vector3 dir = followCam.transform.forward;
            dir.y = 0;

            AddVelocity(dir * PlayerManager.settings.globalDashStrength);
        }

        if (controllerCache != ControllerInput.available)
        {
            controllerCache = ControllerInput.available;
            UpdateCameraPosition(followCam, ControllerInput.available);
        }

        if (transform.position.y <= -3.3)
        {
            transform.position = startPos;
            velocity = Vector3.zero;
            dashing = false;
        }

        {
            RaycastHit hit;

            int layerMask = 1 << 9;
            layerMask = ~layerMask;

            float distance = 3.16f;

            Vector3 origin = cameraAnchor.position;
            Vector3 direction = followCam.transform.forward * -1;
            Vector3 final = origin + (direction * distance);

            if (PlayerManager.settings.debug) Debug.DrawLine(origin, final, Color.red);

            if (Physics.Raycast(origin, direction, out hit, distance, layerMask))
            { 
                followCam.transform.position = hit.point; 
            }
            else followCam.transform.position = final;
        }
    }
    #endregion
}
