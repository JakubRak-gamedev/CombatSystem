using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float MaxMoveSpeed = 10f;
    public float GroundDrag = 5f;
    public float dashSpeed;
    public float jumpForce;
    public float jumpCooldown;
    public float AirMulti;

    bool readyTojump = true;
    bool disableGravity = false;

    public int jumpsLeft = 1;

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;
  

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask WhatIsGround;
    
    public bool dashing;


    public Transform Orientation;

    float HorizontalInput;
    float VerticalInput;
    Vector3 MoveDirection;
    Rigidbody rb;
    float moveSpeed;
    public bool bGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = MaxMoveSpeed;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        bGrounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, WhatIsGround);

        MyInput();
        SpeedControl();

        if(dashing)
        {
            moveSpeed = dashSpeed;
            disableGravity = true;


        }
        else
        {
            moveSpeed = MaxMoveSpeed;
            disableGravity = false;
        }
        // handle drag

        if (bGrounded && !dashing)
        {
            rb.drag = GroundDrag;

        }
        else
        {
            rb.drag = 0;
        }

        if(!bGrounded && !disableGravity)
        {
            rb.AddForce(rb.transform.up * -2f);
        }

    }
    private void MyInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpkey) && bGrounded && readyTojump)
        {

            Jump();

            readyTojump = false;

            Invoke(nameof(ResetJump), 0.025f);

            return;
        }

        if (Input.GetKeyDown(jumpkey) && (jumpsLeft > 0) ) //&& readyTojump
        {

            Jump();
            jumpsLeft = jumpsLeft - 1;
        
        }

        
       
    }

    private void MovePlayer()
    {
        MoveDirection = Orientation.forward * VerticalInput + Orientation.right * HorizontalInput;

        if(bGrounded)
        {
            rb.AddForce(MoveDirection.normalized * 10f * moveSpeed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(MoveDirection.normalized * 10f * moveSpeed * AirMulti, ForceMode.Force);
        }


    }

    private void SpeedControl()
    {
        Vector3 Vel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 jump = new Vector3(0, rb.velocity.y, 0);        

        if(Vel.magnitude > moveSpeed)
        {
            Vector3 LimitedVel = Vel.normalized * moveSpeed;
            rb.velocity = new Vector3(LimitedVel.x, rb.velocity.y, LimitedVel.z);
            if (jump.magnitude > MaxMoveSpeed)
            {
                Vector3 LimitedjumpVel = jump.normalized * MaxMoveSpeed;
                rb.velocity = new Vector3(LimitedVel.x, LimitedjumpVel.y, LimitedVel.z);
            }
        }
        
    }

    void Jump()
    {
        if(!readyTojump) return;

        
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(rb.transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {

        readyTojump = true;

        jumpsLeft++;
        if (jumpsLeft >= 1)
        {
            jumpsLeft = 1;
        }
    }
}
