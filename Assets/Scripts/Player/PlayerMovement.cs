using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Config")]
    public float moveSpeed;
    public float groundDrag;
    public Transform playerOrientation;

    //inputs
    float horizontalInput;
    float veritcalInput;

    //cached
    Vector3 movementDirection;
    Rigidbody rig_body;

    private void Start()
    {
        rig_body = GetComponent<Rigidbody>();
        rig_body.freezeRotation = true;
    }

    private void Update()
    {
        Player_Input();
        rig_body.drag = groundDrag;
    }

    private void FixedUpdate()
    {
        Player_Move();
        Speed_Control();
    }

    private void Player_Input()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        veritcalInput = Input.GetAxisRaw("Vertical");
    }

    private void Player_Move()
    {
        //only affect X and Z axis
        Vector3 forward = new Vector3(playerOrientation.forward.x, 0f, playerOrientation.forward.z).normalized;
        Vector3 right = new Vector3(playerOrientation.right.x, 0f, playerOrientation.right.z).normalized;

        movementDirection = forward * veritcalInput + right * horizontalInput;

        rig_body.AddForce(10f * moveSpeed * movementDirection.normalized, ForceMode.Force);

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
