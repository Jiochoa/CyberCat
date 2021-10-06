using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// All The phisics that make the character Move
/// </summary>
public class PlayerController1 : MonoBehaviour
{
	//Config
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	//[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent; 
	
	// Slingshot jump Config
	[Header("Slingshot Jump Config")]
	public float power = 10f;
	TrajectoryLine tl;

	public Vector2 minPower;
	public Vector2 maxPower;

	Camera myCamera;
	Vector2 force;
	Vector3 startPoint;
	Vector3 endPoint;
	//bool enableControl = true;

	// Push and Grab
	[Header("Push & Grab")]
	public float distance = 1f;
	public LayerMask boxMask;
	GameObject box;
	public bool beingPushed;
	float xPos;



	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		// Slingshot Config
		myCamera = Camera.main;
		tl = GetComponent<TrajectoryLine>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		xPos = transform.position.x;

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
					OnLandEvent.Invoke();
			}
		}

	}
    private void Update()
    {
		SlingshotJump();
		Push();
		Pull();
	}
    public void Move(float move, bool jump, bool action)
	{

		// only control the player if grounded or airControl is turned on
		//if ((m_Grounded || m_AirControl) && !Input.GetMouseButton(0))
		if ((m_Grounded) && !Input.GetMouseButton(0))
		{
			HorizontalMove(move);
        }
        // If the player should jump...
        if (m_Grounded && jump && !Input.GetMouseButton(0))
        {
            Jump();
        }

	}
    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	private void HorizontalMove(float move)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(
            m_Rigidbody2D.velocity,
            targetVelocity,
            ref m_Velocity,
            m_MovementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }
	private void Jump()
    {
        // Add a vertical force to the player.
        m_Grounded = false;
        m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
    }
	private void SlingshotJump()
    {
		bool mouseButtonIsPressed = Input.GetMouseButtonDown(0);
		if (mouseButtonIsPressed && m_Grounded)
		{
			startPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
			startPoint.z = 15f;
		}

		bool mouseButtonIsHeldDown = Input.GetMouseButton(0);
		if (mouseButtonIsHeldDown && m_Grounded)
		{
			Vector3 curentPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
			curentPoint.z = 15;
			tl.RenderLine(startPoint, curentPoint);
		}

		bool mouseButtonIsReleased = Input.GetMouseButtonUp(0);
		if (mouseButtonIsReleased && m_Grounded)
		{
			endPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
			startPoint.z = 15f;

			force = new Vector2(
				Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
				Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
			m_Rigidbody2D.AddForce(force * power, ForceMode2D.Impulse);
			tl.EndLine();

		}
	}
	private void Push()
    {
		Physics2D.queriesStartInColliders = false;
		RaycastHit2D hit = Physics2D.Raycast(
			transform.position,
			Vector2.right * transform.localScale.x,
			distance,
			boxMask);

		if(hit.collider != null 
			&& hit.collider.gameObject.tag == "Movable" 
			&& Input.GetKeyDown(KeyCode.LeftShift))
        {
			box = hit.collider.gameObject;

			box.GetComponent<FixedJoint2D>().enabled = true;
			beingPushed = true;

			box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();


		} else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
			box.GetComponent<FixedJoint2D>().enabled = false;
			beingPushed = false;
        }

	}
	private void Pull()
    {
		xPos = transform.position.x;

		if(beingPushed == false)
        {
			transform.position = new Vector3(xPos, transform.position.y);
        } else
        {
			xPos = transform.position.x;
        }
    }
    private void OnDrawGizmos()
    {
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(
			transform.position,
			(Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }

}
