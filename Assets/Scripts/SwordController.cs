using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

[ExecuteInEditMode]
public class SwordController : SignalHandler
{
    /// <summary>
    /// How much "up" force is added to the force vector on the ball
    /// </summary>
    public float upwardForce;

    /// <summary>
    /// How much "up" force is added to the force vector on the ball
    /// </summary>
    public float forwardForce;
    
    /// <summary>
    /// Multiplies the (normalized) force vector on the ball
    /// </summary>
    public float lobHitForce;

    /// <summary>
    /// Multiplies the (normalized) force vector on the ball
    /// </summary>
    public float kickHitForce;

    /// <summary>
    /// Moves the "hit box" for hitting the ball. this is the white circle.
    /// </summary>
    public Vector2 overlapCenter;
    
    /// <summary>
    /// "hit box" circle radius
    /// </summary>
    public float overlapRadius = 0.5f;
    
    /// <summary>
    /// Layers that will be treated as a ball that can be hit.
    /// </summary>
    public LayerMask whatIsHittable;

    /// <summary>
    /// Whether or not this is the frame that the ball was hit. Essentially a flag that represents "OnHitEnter" or "OnHitExit"
    /// </summary>
    bool ballHasBeenHitThisTime = false;

    /// <summary>
    /// Whether or not ball is trapped in this frame.
    ///</summary>
    bool trappedBall = false;

    public GameObject player;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)overlapCenter, overlapRadius);
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        bool foundBallFlag = false;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + (Vector3)overlapCenter, overlapRadius, whatIsHittable);
        foreach(Collider2D hit in hits)
        {
            foundBallFlag = true;
            if(!ballHasBeenHitThisTime && Input.GetButtonDown("Lob"))
            {
                ballHasBeenHitThisTime = true;
                hit.transform.GetComponent<Rigidbody2D>().AddForce(((hit.transform.position - this.transform.position) + Vector3.up * upwardForce).normalized * lobHitForce, ForceMode2D.Impulse);
            }
            if (!ballHasBeenHitThisTime && Input.GetButtonDown("Kick"))
            {
                ballHasBeenHitThisTime = true;
                hit.transform.GetComponent<Rigidbody2D>().AddForce(((hit.transform.position - this.transform.position) + (player.transform.right) * forwardForce).normalized * kickHitForce, ForceMode2D.Impulse);
                SendSignal("PlayerKicked");
            
            }

            var playerController = player.GetComponent<PlayerController>();

            if(!ballHasBeenHitThisTime && Input.GetButton("Trap") && (playerController.jumpState != PlayerController.JumpState.Grounded && !playerController.touchingWall))
            {
                ballHasBeenHitThisTime = true;
                hit.transform.parent = player.transform.GetChild(0).transform;
                hit.transform.position = player.transform.GetChild(0).transform.position;
                hit.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                hit.transform.GetComponent<Rigidbody2D>().isKinematic = true;
                hit.transform.GetComponent<CircleCollider2D>().isTrigger = true;
                trappedBall = true;
                SendSignal("PlayerTrapped");
            }
            if(trappedBall){
                if (playerController.jumpState == PlayerController.JumpState.Grounded || playerController.touchingWall)
                {
                    hit.transform.parent = null;
                    hit.transform.GetComponent<Rigidbody2D>().isKinematic = false;
                    hit.transform.GetComponent<CircleCollider2D>().isTrigger = false;
                    trappedBall = false;
                }else if(ballHasBeenHitThisTime && Input.GetButtonDown("Kick"))
                {
                    ballHasBeenHitThisTime = true;
                    hit.transform.parent = null;
                    hit.transform.GetComponent<Rigidbody2D>().isKinematic = false;
                    hit.transform.GetComponent<CircleCollider2D>().isTrigger = false;
                    hit.transform.GetComponent<Rigidbody2D>().AddForce(((hit.transform.position - this.transform.position) + (player.transform.right) * forwardForce).normalized * kickHitForce, ForceMode2D.Impulse);
                    trappedBall = false;
                    SendSignal("PlayerKicked");
                }else if(ballHasBeenHitThisTime && Input.GetButtonUp("Trap"))
                {
                    hit.transform.parent = null;
                    hit.transform.GetComponent<Rigidbody2D>().isKinematic = false;
                    hit.transform.GetComponent<CircleCollider2D>().isTrigger = false;
                    trappedBall = false;
                }
            }
            
        }

        if(!foundBallFlag)
        {
            ballHasBeenHitThisTime = false;
        }
    }
}
