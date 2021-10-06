using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tell the player when to move and at what speed
/// </summary>
public class PlayMovement : MonoBehaviour
{
    // Catche
    public PlayController controller;
    public Animator animator;
    public LineRenderer lineRenderer;

    float horizontalMove = 0f;
    public float runSpeed = 10f;
    bool jump = false;
    Vector2 longJump;
    GameObject validObstacle = null;
    //bool action = false;


    // Player input Commands
    private string inputHorizontal = "Horizontal";
    //private string inputVertical = "Vertical";
    private string inputHoldJump = "Hold Jump";
    private string inputJump = "Jump";
    private string inputAction = "Action";

    [Header("Long Jump")]
    public float power = 10f;
    public Vector2 minPower;
    public Vector2 maxPower;

    Camera myCamera;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    // Push and Grab
    [Header("Push & Grab")]
    public float distance = 1f;
    public LayerMask boxMask;
    GameObject box;
    RaycastHit2D hit;
    //public bool beingPushed;
    //float xPos;

    // LedgeClimb
    [Header("Ledge Climb")]
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;
    private bool ledgeDetected;
    private bool canClimbLedge = false;
    private bool isFacingRight = true;
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;
    public float wallCheckDistance;
    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;
    private bool canMove;
    private bool canFlip;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    private void Start()
    {
        myCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        //tl = GetComponent<TrajectoryLine>();
        //xPos = transform.position.x;

    }


    void Update()
    {

        CheckLedgeClimb();


        // character reach for grabbing
        Physics2D.queriesStartInColliders = false;
        hit = Physics2D.Raycast(
            transform.position,
            Vector2.right * transform.localScale.x,
            distance,
            boxMask);

        // check is valid obstacle is within characters reach
        if (Input.GetButtonDown(inputAction)
            && hit.collider != null
            && hit.collider.tag == "Movable"
            && !Input.GetButton(inputHoldJump))
        {
            // send box object to controller
            validObstacle = hit.collider.gameObject;
        } else if(Input.GetButtonUp(inputAction))
        {
            validObstacle = null;
        }

        // Player movement horizontal
        if (Input.GetButton(inputHorizontal) && !Input.GetButton(inputHoldJump))
        {
            horizontalMove = Input.GetAxis(inputHorizontal) * runSpeed;
            //animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        } else
        {
            horizontalMove =  0;
        }

        // Player movement jump
        if (Input.GetButtonDown(inputJump) && !Input.GetButton(inputHoldJump))
        {

            jump = true;
            //animator.SetBool("isJumping", true);
        }



        //// LongJump ////
        bool mouseButtonIsPressed = Input.GetMouseButtonDown(0);
        if (mouseButtonIsPressed)
        {
            startPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15f;
        }

        bool mouseButtonIsHeldDown = Input.GetMouseButton(0);
        if (mouseButtonIsHeldDown)
        {
            Vector3 curentPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
            curentPoint.z = 15;
            RenderLongJumpLine(startPoint, curentPoint);
        }

        bool mouseButtonIsReleased = Input.GetMouseButtonUp(0);
        if (mouseButtonIsReleased)
        {
            endPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15f;

            force = new Vector2(
                Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
                Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            longJump = force * power;
            EndLongJumpLine();

        }

    }


    public void OnLanding()
    {
        //animator.SetBool("isJumping", false);
    }

    private void FixedUpdate()
    {
        // Move out character
        controller.Move(
            horizontalMove * Time.fixedDeltaTime, 
            jump, 
            validObstacle, 
            longJump);
        jump = false;
        //action = false;
        //action = null;
        longJump = new Vector2();

        CheckSurroundings();
    }

    public void RenderLongJumpLine(Vector3 startPoint, Vector3 endPoint)
    {
        lineRenderer.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = endPoint;

        lineRenderer.SetPositions(points);
    }

    public void EndLongJumpLine()
    {
        lineRenderer.positionCount = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            transform.position,
            (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
    
    // charc
    private void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if (isFacingRight)
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
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, 
                    Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, 
                    Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }

            //canMove = false;
            //canFlip = false;

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
        //canMove = true;
        //canFlip = true;
        ledgeDetected = false;
        animator.SetBool("canClimbLedge", canClimbLedge);
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }
}
