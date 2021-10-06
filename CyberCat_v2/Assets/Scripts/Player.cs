using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;


    // State
    float gravityScaleAtStart;


    // Cached component reference
    Rigidbody2D myRigidbody2D;
    Animator myAnimator;
    //CapsuleCollider2D myColider;
    BoxCollider2D myBodyCollider;
    CapsuleCollider2D myFeetCollider;

    [Header("Slingshot Config")]
    // Slingshot conf
    public float power = 10f;
    TrajectoryLine tl;

    public Vector2 minPower;
    public Vector2 maxPower;

    Camera myCamera;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;
    bool enableControl = true;
    private bool m_Grounded;
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character



    // Message then methods
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        myFeetCollider = GetComponent<CapsuleCollider2D>();
        //myColider = GetComponent<CapsuleCollider2D>();
        myCamera = Camera.main;
        tl = GetComponent<TrajectoryLine>();

        gravityScaleAtStart = myRigidbody2D.gravityScale;


    }

    // Update is called once per frame
    void Update()
    {

       
        Run();
        // ClimbLadder();
        Jump();
        SlingShotJump();
        FlipSprite();
       
        

    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    //OnLandEvent.Invoke();
                }
            }
        }
    }
    private void Run()
    {
        if (enableControl)
        {
            float controlThrow = Input.GetAxis("Horizontal");
            Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody2D.velocity.y);
            myRigidbody2D.velocity = playerVelocity;

            bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
            //myAnimator.SetBool("Running", playerHasHorizontalSpeed);
        }
    }

    private void ClimbLadder()
    {
    
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            myAnimator.SetBool("Climbing", false);
            myRigidbody2D.gravityScale = gravityScaleAtStart;
            return; 
        }

        // change velocity

        float controlThrow = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidbody2D.velocity.x, controlThrow * climbSpeed);
        myRigidbody2D.velocity = climbVelocity;
        myRigidbody2D.gravityScale = 0f;

        // change animation
        //myAnimator.SetBool("Climbing", playerReachLatter);
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody2D.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
        

        



    }

    private void Jump()
    {
        //bool jumpButtonIsPressed = Input.GetButtonDown("Jump");
        //bool isTouchingGround = myColider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (enableControl)
        {

            if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) 
            {
                return; 
            }


            if (Input.GetButtonDown("Jump"))
            {
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                myRigidbody2D.velocity += jumpVelocityToAdd;
            }

        }
    }

    private void FlipSprite()
    {
        if (enableControl)
        {
            bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
            // if player is moving horizontally
            if(playerHasHorizontalSpeed)
            {
                // reverse the current x axis
                transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
            }

        }
    }

    private void SlingShotJump()
    {
        

        if (m_Grounded && Input.GetMouseButtonDown(0))
        {
            // turn off controllers
            enableControl = false;

            startPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15f;
        }

        if (m_Grounded && Input.GetMouseButton(0))
        {
            Vector3 curentPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
            curentPoint.z = 15;
            tl.RenderLine(startPoint, curentPoint);
        }

        if (m_Grounded && Input.GetMouseButtonUp(0))
        {
            endPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15f;

            force = new Vector2(
                Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
                Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            
            m_Grounded = false;
            myRigidbody2D.AddForce(force * power, ForceMode2D.Impulse);
            
            tl.EndLine();
        }

        bool playerHasLanded = m_Grounded;
        if(playerHasLanded)
        {
            // turn on controllers
            enableControl = true;
        }


    }

}
