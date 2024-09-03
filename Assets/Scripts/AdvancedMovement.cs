using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AdvancedMovement : MonoBehaviour
{
    public Transform orientation;
    public PlayerMovement pm;

    private Rigidbody rb;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;
    public KeyCode DashKey;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    float HorizontalInput;
    float VerticalInput;

    bool grounded;

    private void MyInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        grounded = pm.bGrounded;

        
        if(Input.GetKeyDown(DashKey))
        {
            Dash();
        }
            
        if(dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.dashing = true;

        Vector3 forcetoApply = rb.velocity.normalized * dashForce + orientation.up * dashUpwardForce;

        DelayedForceToApply = forcetoApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

       
        Invoke(nameof(ResetDash), dashDuration);

    }

    private Vector3 DelayedForceToApply;

    private void DelayedDashForce()
    {
        rb.AddForce(DelayedForceToApply, ForceMode.Impulse);

        pm.ResetJump();

    }

    void ResetDash()
    {
        pm.dashing = false;
    }
}
