using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeClimb : MonoBehaviour
{

    // LedgeClimb
    public Transform ledgeCheck;
    private bool isTouchingLedge;
    private bool canClimbLedge = false;
    private bool ledgeDetected;
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;
    private float ledgeClimbXOffset1 = 0f;
    private float ledgeClimbYOffset1 = 0f;
    private float ledgeClimbXOffset2 = 0f;
    private float ledgeClimbYOffset2 = 0f;

    // Not yet here
    Animator animator = null;
    bool isTouchingWall = false;
    Transform wallCheck = null;
    bool isFaceingRight = false;
    float wallCheckDistance = 0f;
    bool canMove = false;
    bool canFlip = false;
    bool movementInputDirection = false;
    bool facingDirection = false;
    bool isWallSliding = false;
    Rigidbody2D rb = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckLedgeClimb();
    }





    private void CheckSurroundings()
    {
        //isTouchingLedge = Physics2D.Raycast(
        //    ledgeCheck.position,
        //    transform.right,
        //    wallCheckDistance,
        //    whatIsGround);

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if (isFaceingRight)
            {
                ledgePos1 = new Vector2(
                    Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1,
                    Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(
                    Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2,
                    Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(
                    Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1,
                    Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(
                    Mathf.Ceil(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset2,
                    Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            canMove = false;
            canFlip = false;

            animator.SetBool("canClimbLedge", canClimbLedge);


        }

        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }

    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        animator.SetBool("canClimbLedge", canClimbLedge);


    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0 && !canClimbLedge)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }



}
