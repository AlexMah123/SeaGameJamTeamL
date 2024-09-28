using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Config")]
    public float moveSpeed;
    public float groundDrag;
    public float gravity = -9.81f;
    public float groundedGravity = -1f;
    public Transform playerOrientation;

    //inputs
    float horizontalInput;
    float veritcalInput;
    bool groundedPlayer;

    //cached
    Vector3 playerVelocity = Vector3.zero;
    Rigidbody rig_body;
    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        rig_body = GetComponent<Rigidbody>();
        rig_body.freezeRotation = true;
    }

    private void Update()
    {
        Player_Input();
        Player_Move();
        rig_body.drag = groundDrag;
    }

    private void FixedUpdate()
    {
        Speed_Control();
    }

    private void Player_Input()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        veritcalInput = Input.GetAxisRaw("Vertical");
    }

    private void Player_Move()
    {
        groundedPlayer = characterController.isGrounded;

        // Calculate movement direction
        Vector3 forward = new Vector3(playerOrientation.forward.x, 0f, playerOrientation.forward.z).normalized;
        Vector3 right = new Vector3(playerOrientation.right.x, 0f, playerOrientation.right.z).normalized;
        Vector3 movementDirection = (forward * veritcalInput + right * horizontalInput).normalized;

        // Apply movement only on X and Z axes
        Vector3 move = movementDirection * moveSpeed;

        // Handle gravity
        if (groundedPlayer)
        {
            if (playerVelocity.y < 0)
            {
                playerVelocity.y = groundedGravity;
            }
        }
        else
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        Vector3 finalMove = move * Time.deltaTime;
        finalMove.y = playerVelocity.y * Time.deltaTime;

        characterController.Move(finalMove);
    }

    private void Speed_Control()
    {
        Vector3 maxVel = new(rig_body.velocity.x, 0f, rig_body.velocity.z);

        if(maxVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = maxVel.normalized * moveSpeed;
            rig_body.velocity = new Vector3(limitedVel.x, rig_body.velocity.y, limitedVel.z);
        }
    }
}
