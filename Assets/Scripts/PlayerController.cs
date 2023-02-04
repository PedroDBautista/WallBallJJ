using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst.CompilerServices;
using System.ComponentModel;
using System;

/// <summary>
/// This is the main class used to implement control of the player.
/// </summary>
[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class PlayerController : KinematicObject
{
    /// <summary>
    /// Max horizontal speed of the player.
    /// </summary>
    public float maxSpeed = 7;
    /// <summary>
    /// Initial jump velocity at the start of a jump.
    /// </summary>
    public float jumpTakeOffSpeed = 7;

    public JumpState jumpState = JumpState.Grounded;
    private bool stopJump;
    public Collider2D collider2d;
    public bool controlEnabled = true;

    private bool isFacingRight = true;

    bool jump;
    Vector2 move;
    SpriteRenderer spriteRenderer;


    [Header("Wall Jumping")]
        
    [SerializeField]
    /// <summary>
    /// Amount of time that player input will be ignored after activating the wall jump.
    /// This is so that the player will have some time to change the direction of movement so they don't cancel their wall jump.
    /// </summary>
    float wallJumpForceTime;
    float wallJumpForceTimeLeft;
    [SerializeField]
    /// <summary>
    /// The initial x velocity of the player after initiating a wall jump.
    /// </summary>
    float wallJumpXVelocity;
    [SerializeField]
    /// <summary>
    /// The initial y velocity of the player after initiating a wall jump.
    /// </summary>
    float WallJumpYVelocity;

    Vector2 wallJumpDirection;


    [Header("Wall Checking")]
    [Description("testy test.")]
    public LayerMask whatIsWall;
    public int wallCheckRayCount = 3;
    /// <summary>
    ///  How far away the player's collider can be before they are technically "touching a wall".
    ///  This allows players to execute wall jump with a bit more ease.
    /// </summary>
    public float wallCheckRayDistance = 0.1f;
    [Range(0,1)]
    /// <summary>
    /// Distance from the top of the attached Collider to stop shooting rays for wall checks.
    /// This parameter will SQUISH the raycasts down toward the first raycast point which is set by the below parameter "wallCheckBottomPadding".
    /// </summary>
    public float wallCheckTopPadding = 0.1f;
    [Range(0,1)]
    public float wallCheckBottomPadding = 0.1f;
        
    bool touchingWallLeft;
    public bool touchingWall = false;


    public Bounds Bounds => collider2d.bounds;

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        wallJumpForceTimeLeft -= Time.deltaTime;
        if (controlEnabled && wallJumpForceTimeLeft <= 0f)
        {
            move.x = Input.GetAxisRaw("Horizontal");
            if ( (jumpState == JumpState.Grounded || touchingWall) && Input.GetButtonDown("Jump"))
                jumpState = JumpState.PrepareToJump;
            else if (Input.GetButtonUp("Jump"))
            {
                stopJump = true;
            }
        }
        else
        {
            move.x = 0;
        }
        CheckMovementDirection();
        UpdateJumpState();
        base.Update();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
        WallCheck();            
    }

    // Flips The Player Sprite to face the right direction when moving
    private void CheckMovementDirection()
    {
        if (isFacingRight && move.x < 0)
        {
            Flip();
        }
        else if (!isFacingRight && move.x > 0)
        {
            Flip();
        }
        /*
        if (velocity.x != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        */
    }

    // Flips the Player Sprite
    private void Flip()
    {
        if (!touchingWall)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    void UpdateJumpState()
    {
        jump = false;
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                jumpState = JumpState.Jumping;
                jump = true;
                stopJump = false;
                SendSignal("PlayerJumped");
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    jumpState = JumpState.InFlight;
                }
                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
                    jumpState = JumpState.Landed;
                }
                break;
            case JumpState.Landed:
                jumpState = JumpState.Grounded;
                break;
        }
    }

    protected override void ComputeVelocity()
    {
        if (jump && touchingWall)
        {
            wallJumpForceTimeLeft = wallJumpForceTime;
            wallJumpDirection.y = WallJumpYVelocity;
            wallJumpDirection.x = touchingWallLeft ? wallJumpXVelocity : -wallJumpXVelocity;
            velocity = wallJumpDirection;
            jump = false;
        }
        else if (jump && IsGrounded)
        {
            velocity.y = jumpTakeOffSpeed;
            jump = false;
        }
        else if (stopJump)
        {
            stopJump = false;
            if (velocity.y > 0)
            {
                //OLD CODE
                velocity.y = velocity.y / 2;
            }
        }


        //just for sprite flipping
        if (move.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (move.x < -0.01f)
            spriteRenderer.flipX = true;

        //OLD CODE
        //animator.SetBool("grounded", IsGrounded);
        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        if(wallJumpForceTimeLeft >= 0)
        {
            //set target velocity from 
            targetVelocity = velocity;
        }
        else
        {
            //set target velocity from 
            targetVelocity = move * maxSpeed;
        }
        Debug.DrawLine(this.transform.position, this.transform.position + (Vector3)velocity, Color.green);
    }

    private void WallCheck()
    {
        // Right

        Vector3 bottomRightOfBox = new Vector3(collider2d.bounds.center.x + collider2d.bounds.extents.x, collider2d.bounds.center.y - collider2d.bounds.extents.y + wallCheckBottomPadding, collider2d.transform.position.z);
        int positiveRightWallchecks = 0;
        for (int i = 0; i < wallCheckRayCount; i++)
        {
            Vector2 lineOrigin = bottomRightOfBox + new Vector3(0f, (i * (collider2d.bounds.size.y - (wallCheckTopPadding)) / (wallCheckRayCount - 1)), 0f);
            Vector2 lineEnd = new Vector2(lineOrigin.x + wallCheckRayDistance, lineOrigin.y);
            RaycastHit2D hit = Physics2D.Linecast(lineOrigin, lineEnd, whatIsWall);
            if (hit)
            {
                positiveRightWallchecks++;
                Debug.DrawLine(lineOrigin, lineEnd, Color.blue);
            }
            else
            {
                Debug.DrawLine(lineOrigin, lineEnd, Color.green);
            }
        }

        // Left

        Vector3 bottomLeftOfBox = new Vector3(collider2d.bounds.center.x - collider2d.bounds.extents.x, collider2d.bounds.center.y - collider2d.bounds.extents.y + wallCheckBottomPadding, collider2d.transform.position.z);
        int positiveLeftWallchecks = 0;
        for (int i = 0; i < wallCheckRayCount; i++)
        {
            Vector2 lineOrigin = bottomLeftOfBox + new Vector3(0f, (i * (collider2d.bounds.size.y - wallCheckTopPadding) / (wallCheckRayCount - 1)), 0f);
            Vector2 lineEnd = new Vector2(lineOrigin.x - wallCheckRayDistance, lineOrigin.y);
            RaycastHit2D hit = Physics2D.Linecast(lineOrigin, lineEnd, whatIsWall);
            if (hit)
            {
                positiveLeftWallchecks++;
                Debug.DrawLine(lineOrigin, lineEnd, Color.blue);
            }
            else
            {
                Debug.DrawLine(lineOrigin, lineEnd, Color.green);
            }
        }

        if (positiveLeftWallchecks > 0)
        {
            touchingWall = true;
            touchingWallLeft = true;
        }

        if(positiveRightWallchecks > 0)
        {
            touchingWall = true;
            touchingWallLeft = false;
        }

        if (positiveLeftWallchecks == 0 && positiveRightWallchecks == 0)
        {
            touchingWall = false;
        }
    }

    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }
}