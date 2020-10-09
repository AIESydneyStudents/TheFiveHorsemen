using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : ControllerInput
{
    private Vector3 moveDirection = Vector3.zero;
    private float xPos = 0f;
    private float yPos = 0f;

    private CharacterController controller;

    [SerializeField] private float moveSpeed = 6.0F;

    public override void Start()
    {
        base.Start();

        controller = GetComponent<CharacterController>();
    }

    private void Move()
    {
        xPos += GetHorizontalAxis();
        yPos += GetVerticalAxis();

        transform.Rotate(0, xPos, 0);

        moveDirection = transform.forward;
        moveDirection = moveDirection * yPos;

        moveDirection.y += Physics.gravity.y * Time.deltaTime;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        xPos = 0;
        yPos = 0;
    }

    void Update()
    {
        Move();
    }
}
