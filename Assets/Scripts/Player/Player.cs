using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private Rigidbody rb;
    public PlayerInput input;
    public FPSCamera cam;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform ledgeCheckPos;
    [SerializeField] private Transform headTransform;
    [SerializeField] private float playerHeight = 2.0f;
    [Header("Movement Speed")]
    public float maxSpeed = 8f;
    public float crouchSpeed = 8f;
    public float wallRunSpeed = 15f;
    public float climbSpeed = 5f;
    public float wallClimbSpeed = 3f;
    [Header("Movement Treshold")]
    public float acceleration = 1.5f;
    public float friction = 2f;
    public float airFriction = 1.2f;
    [Header("Mobility")]
    [Header("Jumping")]
    public float jumpStrength = 12f;
    public Vector2 wallJumpForces = new Vector2(15f, 20f);
    [Header("Crouch and Slide")]
    public float crouchYScale = 0.5f;
    public float startYScale = 1f;
    public float slideTime = 1f;
    public float slideStrength = 50f;
    private RaycastHit ceilHit;
    [Header("Dashing")]
    public float dashStrength = 20f;
    public float dashTime = 0.5f;
    [Header("Wall running")]
    public float wallRunTime = 1f;
    public float wallCheckDistance = 0.7f;
    private RaycastHit rightWallHit;
    private RaycastHit leftWallHit;
    private bool leftWall;
    private bool rightWall;
    public bool frontWall { private set; get; }
    public bool canWallRun = true;
    [Header("Slope Control")]
    public float maxSlopeAngle = 45f;
    private RaycastHit slopeHit;

    private Vector2 inputDir = Vector2.zero;
    private Vector3 moveDir = Vector3.zero;
    public bool isGrounded { private set; get; }
    public bool isOnCeiling { private set; get; }
    public bool isOnWall { private set; get; }
    public bool isOnSlope { private set; get; }
    public bool isOnLedge { private set; get; }

    private void Start()
    {
        startYScale = transform.localScale.y;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (transform.position.y < -1)
        {
            transform.position = new Vector3(0, 5, 0);
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        isOnCeiling = Physics.SphereCast(transform.position, 0.6f, Vector3.up, out ceilHit, playerHeight / 2 + 0.2f, groundMask)
                        || Physics.Raycast(transform.position, Vector3.up, playerHeight / 2 + 0.3f, groundMask);
        rightWall = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wallMask);
        leftWall = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wallMask);
        frontWall = Physics.Raycast(transform.position, orientation.forward, wallCheckDistance, wallMask);

        isOnLedge = !Physics.Raycast(ledgeCheckPos.position, orientation.forward, wallCheckDistance, wallMask) && frontWall;

        isOnWall = leftWall || rightWall;


        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            isOnSlope = angle < maxSlopeAngle && angle != 0;
        }
        else
        {
            isOnSlope = false;
        }

     

        rb.useGravity = !isOnSlope;
    }

    private void FixedUpdate()
    {
        if (isOnSlope && rb.velocity.y > 0)
        {
            rb.AddForce(Vector3.down * 15f, ForceMode.Force);
        }
    }

    public Vector2 GetInputDir()
    {
        inputDir = input.actions["Movement"].ReadValue<Vector2>();
        return inputDir;
    }

    public Vector3 GetMoveDirection()
    {
        Vector2 input = GetInputDir();
        Vector3 dir = orientation.forward * input.y + orientation.right * input.x;

        if (isOnSlope)
        {
            moveDir = Vector3.ProjectOnPlane(dir, slopeHit.normal);
        }
        else
        {
            moveDir = dir;
        }

        return moveDir.normalized;
    }

    public Vector3 GetPlayerVelocity()
    {
        return rb.velocity;
    }

    public void MovePlayer(Vector3 direction, float speed, float amount)
    {
        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(direction.x * speed, rb.velocity.y, direction.z * speed), amount * Time.fixedDeltaTime);
    }

    public void DoJump(float strength)
    {
        rb.velocity = new Vector3(rb.velocity.x, strength, rb.velocity.z);
    }

    public void DoVault(float strength)
    {
        rb.velocity = Vector3.zero;
        rb.velocity = Vector3.up * strength;
    }

    public void DoCrouch(float yScale)
    {
        transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);
    }

    public void DoDash(Vector3 direction, float amount)
    {
        rb.velocity = Vector3.zero;
        rb.velocity = direction * amount;
    }

    #region Wall Running
    public Vector3 GetWallNormal()
    {
        Vector3 wallNormal = rightWall ? rightWallHit.normal : leftWallHit.normal;
        return wallNormal;
    }

    public Vector3 GetWallForward()
    {
        Vector3 wallForward = Vector3.Cross(GetWallNormal(), transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        return wallForward;
    }

    public void DoWallRun(float speed, float amount)
    {
        if (leftWall) cam.TiltCam(-8 * Mathf.Deg2Rad);
        else if (rightWall) cam.TiltCam(8 * Mathf.Deg2Rad);

        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.velocity = Vector3.Lerp(rb.velocity, GetWallForward() * speed, amount * Time.fixedDeltaTime);

        
        //rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);

        if (!(leftWall && GetInputDir().x > 0f) && !(rightWall && GetInputDir().x < 0f))
        {
            rb.AddForce(-GetWallNormal() * 100f, ForceMode.Force);
        }
    }

    public void DoWallJump(float hJump = 0, float vJump = 0)
    {
        Vector2 forceToJump = wallJumpForces;

        if (hJump != 0 || vJump != 0)
        {
            forceToJump = new Vector2(hJump, vJump);
        }

        Vector3 wallNormal = GetWallNormal() + GetWallForward();
        Vector3 forceToAdd = transform.up * forceToJump.y + wallNormal * forceToJump.x;

        rb.velocity = forceToAdd;
    }
    #endregion

    public void DoBobbing(float amount, float frequency)
    {
        headTransform.transform.position = Vector3.up * Mathf.Sin(amount * frequency);
    }

    public void EnableGravity(bool enable = true)
    {
        rb.useGravity = enable;
    }

    public void SetVelocity(Vector3 newVel)
    {
        rb.velocity = newVel;
    }
}
