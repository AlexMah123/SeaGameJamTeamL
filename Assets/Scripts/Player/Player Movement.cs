using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float groundDrag;
    public Transform ornt;
    float horizontalinput;
    float veritcalinput;
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
        horizontalinput = Input.GetAxisRaw("Horizontal");
        veritcalinput = Input.GetAxisRaw("Vertical");
    }

    private void Player_Move()
    {
        movementDirection = ornt.forward *  veritcalinput + ornt.right * horizontalinput;
        rig_body.AddForce(movementDirection.normalized * moveSpeed * 10f,ForceMode.Force);

    }

    private void Speed_Control()
    {
        Vector3 maxVel = new Vector3(rig_body.velocity.x,0f,rig_body.velocity.z);
        if(maxVel.magnitude>moveSpeed)
        {
            Vector3 limitedVel = maxVel.normalized * moveSpeed;
            rig_body.velocity = new Vector3(limitedVel.x,rig_body.velocity.y,limitedVel.z);
        }
    }
}
