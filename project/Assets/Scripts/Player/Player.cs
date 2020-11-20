using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : ControllerInput
{
    #region Variables
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private bool dashing = false;

    private float xPos = 0f;
    private float yPos = 0f;
    private int controllerCache = 0;

    private CharacterController controller;
    private Vector3 startPos;
    private Vector3 startCamPos;

    private float pushCooldown = 0;
    private bool jumping = false;

    private bool dead = false;
    private float respawnTimer = -1;
    private GameObject rag = null;

    [HideInInspector] public bool ragdolled = false;
    [HideInInspector] public float ragdollTimer = 0;

    private bool climbing = false;

    private bool finished = false;
    #endregion

    #region Editor Fields
    [SerializeField] private ParticleSystem dashPuff;
    [SerializeField] private ParticleSystem pushPuff;
    [SerializeField] private Animator animator_test_A;
    [SerializeField] private Camera followCam;
    [SerializeField] private float abilityCooldown = 3;
    [SerializeField] private Transform cameraAnchor;
    [SerializeField] private Transform character;
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private Slider cooldown;
    [SerializeField] private GameObject wonOverlay;
    #endregion

    public override void Start()
    {
        base.Start();

        startPos = transform.position;

        startCamPos = followCam.transform.localPosition;

        controller = GetComponent<CharacterController>();

        cooldown.maxValue = abilityCooldown;
        transform.Rotate(0, -90, 0);
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
    /// Set player velocity.
    /// </summary>
    /// <param name="vel">Vector3 directional velocity to set.</param>
    public void SetVelocity(Vector3 vel)
    {
        velocity = vel;
    }

    /// <summary>
    /// Ragdoll the player.
    /// </summary>
    public void Ragdoll()
    {
        if (ragdolled)
        {
            Destroy(rag);
            character.gameObject.SetActive(true);
            rag = null;
            ragdolled = false;
        }
        else
        {
            rag = Instantiate(ragdoll, transform);
            character.gameObject.SetActive(false);
            ragdolled = true;
        }
    }

    /// <summary>
    /// Death functionality
    /// </summary>
    /// <param name="respawnTime">Time to respawn.</param>
    public void Die(float respawnTime)
    {
        if (respawnTimer > 0)
        {
            respawnTimer -= Time.deltaTime;

            return;
        }
        else if (respawnTimer > -1)
        {
            transform.position = startPos;
            respawnTimer = -1;
            dead = false;
            ragdollTimer = 0;
            velocity = Vector3.zero;

            if (ragdolled) Ragdoll();

            return;
        }

        respawnTimer = respawnTime;
        velocity = Vector3.zero;
        dashing = false;
        dead = true;

        if (!ragdolled) Ragdoll();
    }

    /// <summary>
    /// Animating the player
    /// </summary>
    private void Animate()
    {
        if (moveDirection.z > 0)
        {
            animator_test_A.SetBool("Running", true);
        }
        else if (moveDirection.z < 0)
        {
            animator_test_A.SetBool("Running", true);
        }
        else
        {
            animator_test_A.SetBool("Running", false);
        }
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
        float canTurn = PlayerManager.settings.globalCanTurn;

        if (ragdollTimer > 0)
        {
            ragdollTimer -= Time.deltaTime;
        }
        else
        {
            if (ragdolled) Ragdoll();

            xPos += GetHorizontalAxis();
            yPos += GetVerticalAxis();
        }

        // Direction
        Vector3 f = followCam.transform.forward;
        Vector3 r = followCam.transform.right;

        f.y = 0f;
        r.y = 0f;

        f.Normalize();
        r.Normalize();
        //

        moveDirection = new Vector3(f.x * yPos + velocity.x, (jumping ? moveDirection.y : 0) + velocity.y, f.z * yPos + velocity.z);

        Vector3 right = r;
        right *= xPos;

        moveDirection = right + moveDirection;

        Animate();

        if (GetJumpButton() && controller.isGrounded && !jumping && !dead && !ragdolled)
        {
            jumping = true;
            moveDirection.y = jumpHeight;

            animator_test_A.SetBool("Running", false);
            animator_test_A.SetBool("Jump", true);
        }
        else if (jumping && controller.isGrounded)
        {
            moveDirection.y = 0;
            jumping = false;
        }
        else animator_test_A.SetBool("Jump", false);

        if (!controller.isGrounded)
        {
            Climb(yPos);
        }

        if (!climbing)
        {
            moveDirection.y -= Time.deltaTime * gravity * (jumping ? 1 : 10);
        }

        if (transform.parent != null && !jumping && !ragdolled && GetHorizontalAxis() == 0 && GetVerticalAxis() == 0 && velocity.x == 0 && velocity.y == 0)
        {
            animator_test_A.SetBool("Running", false);
            animator_test_A.SetBool("Idle", true);
        }
        else controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        //followCam.transform.localPosition = new Vector3(followCam.transform.localPosition.x, startCamPos.y + (-3 * GetRVerticalAxis()), followCam.transform.localPosition.z);

        //followCam.transform.LookAt(cameraAnchor);
        //followCam.transform.RotateAround(transform.position, new Vector3(0.0f, 1.0f, 0.0f), turnSpeed * GetRHorizontalAxis());

        //Vector3 rot = followCam.transform.eulerAngles;
        //rot.x = 0;
        //rot.y += 90f;
        //rot.eulerAngles = new Vector3(0, rot.eulerAngles.y + turnSpeed * GetHorizontalAxis() * Time.deltaTime, 0 );

        float turnRate = new Vector2(GetHorizontalAxis(), GetVerticalAxis()).sqrMagnitude;
        if (canTurn <= turnRate)
        {
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), turnSpeed / 10);
            rot.eulerAngles = new Vector3(0, rot.eulerAngles.y, 0);

            transform.rotation = rot;
        }

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
            pushPuff.transform.position = hit.point;
            pushPuff.Play();

            Player rb;

            if (hit.transform.TryGetComponent(out rb))
            {
                rb.AddVelocity(dir * PlayerManager.settings.globalPushStrength);
                if (!rb.ragdolled) rb.Ragdoll();

                rb.ragdollTimer = 3;
            }
        }
    }

    /// <summary>
    /// Climbing.
    /// </summary>
    public void Climb(float forward)
    {
        RaycastHit hit;

        int layerMask = 1 << 10;

        Vector3 dir = followCam.transform.forward;
        dir.y = 0;

        if (Physics.Raycast(transform.position, dir, out hit, 0.5f, layerMask))
        {
            climbing = true;
            jumping = false;
            moveDirection.x = 0;
            moveDirection.z = 0;
            moveDirection.y += 30 * forward * Time.deltaTime;

            animator_test_A.SetBool("Jump", false);

            if (forward == 0)
            {
                animator_test_A.SetBool("Running", false);
                animator_test_A.SetBool("Climb", false);
            }
            else animator_test_A.SetBool("Climb", true);
        }
        else
        {
            if (climbing)
                moveDirection.y += 5;

            climbing = false;
            animator_test_A.SetBool("Climb", false);
        }
    }

    public void EndGame()
    {
        wonOverlay.SetActive(true);
        finished = true;
    }

    /// <summary>
    /// Creating dust.
    /// </summary>
    private void CreateDust()
    {
        dashPuff.Play();
    }

    /// <summary>
    /// Function runner.
    /// </summary>
    void FixedUpdate()
    {
        if (GetBackButton())
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
            gameObject.SetActive(false);

            return;
        }

        if (finished) return;
        if (!PlayerManager.settings.debug && !controllerExists) gameObject.SetActive(false);

        if (dead || transform.position.y <= -3.3)
        {
            Die(3);

            return;
        }

        Move();

        if (pushCooldown > 0) pushCooldown -= Time.deltaTime;
        cooldown.value = Mathf.Clamp(abilityCooldown-pushCooldown, 0, abilityCooldown);

        if (dashing)
        {
            if (pushCooldown <= (abilityCooldown - 0.5f))
            { 
                dashing = false;
            }
            Push();
            CreateDust();
        }


        if (!dashing && Clicking() && pushCooldown <= 0 && !dead && !ragdolled)
        {
            pushCooldown = abilityCooldown;
            dashing = true;

            Vector3 dir = followCam.transform.forward;
            dir.y = 0;

            AddVelocity(dir * PlayerManager.settings.globalDashStrength);
            animator_test_A.SetBool("Run_Push", true);
        }
        else
        {
            animator_test_A.SetBool("Run_Push", false);
        }

        if (controllerCache != ControllerInput.available)
        {
            controllerCache = ControllerInput.available;
            UpdateCameraPosition(followCam, ControllerInput.available);

            //Rect cRect = followCam.rect;
            RectTransform sliderRect;
            RectTransform canvasRect;

            Transform parent = cooldown.transform.parent;

            if (parent.TryGetComponent(out canvasRect) && cooldown.TryGetComponent(out sliderRect))
            {
                sliderRect.anchoredPosition = new Vector2(canvasRect.rect.width - sliderRect.rect.width/1.5f, -sliderRect.rect.height/2);
            }
        }

        {
            //RaycastHit hit;

            //int layerMask = 1 << 9;
            //layerMask = ~layerMask;

            //float distance = 3.16f;

            //Vector3 origin = cameraAnchor.position;
            //Vector3 direction = followCam.transform.forward * -1;
            //Vector3 final = origin + (direction * distance);

            //if (PlayerManager.settings.debug) Debug.DrawLine(origin, final, Color.red);

            //if (Physics.Raycast(origin, direction, out hit, distance, layerMask))
            //{ 
            //    followCam.transform.position = hit.point; 
            //}
            //else followCam.transform.position = final;
        }
    }
    #endregion
}
