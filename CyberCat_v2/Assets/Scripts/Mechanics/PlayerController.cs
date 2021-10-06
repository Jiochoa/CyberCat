using UnityEngine;
using UnityEngine.Events;

namespace Platformer.Mechanics
{
	public class PlayerController : MonoBehaviour
	{

		// Player Controller default vars
		// -- CATCHE --
		[Header("Audio Settings")]
		public AudioClip jumpAudio;
		public AudioClip respawnAudio;
		public AudioClip ouchAudio;
		/*internal new*/
		public Collider2D collider2d;
		/*internal new*/
		public AudioSource audioSource;
		public Health health;
		public bool controlEnabled = false;
		// The current velocity of the entity.
		public Vector2 velocity;
		public JumpState jumpState = JumpState.Grounded;
		public Animator animator;


		[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
		[SerializeField] private LayerMask whatIsGround;
		//[SerializeField] private LayerMask whatIsObstacle;
		[SerializeField] private LayerMask whatIsButton;
		[SerializeField] private Transform groundCheck;
		[SerializeField] private Transform wallCheck;
		[SerializeField] private Transform ledgeCheck;
		[SerializeField] private CapsuleCollider2D myFeet;


		// Catche
		private Rigidbody2D m_Rigidbody2D;


		//private CapsuleCollider2D myFeet;

		private bool isGrounded;
		private bool isTouchingWall;
		private bool isTouchingLedge;
		private bool isTouchingObstacle;
		private bool isTouchingButton;
		private bool ledgeDetected;
		private bool canMove = true;
		private bool canFlip = true;
		


		//public float groundCheckRadius;
		public float reachDistance = 1f;
		float wallcheckReach;
		float ledgecheckReach;
		private bool isFacingRight = true;
		private Vector3 m_Velocity = Vector3.zero;

		private bool canClimbLedge = false;
		private Vector2 ledgePosBot;
		private Vector2 ledgePos1;
		private Vector2 ledgePos2;
		public float ledgeClimbXOffset1 = 0f;
		public float ledgeClimbYOffset1 = 0f;
		public float ledgeClimbXOffset2 = 0f;
		public float ledgeClimbYOffset2 = 0f;

		//RaycastHit2D playerReach;
		GameObject objectCatched;

		//public float reachDistance = 1f;
		[Header("Events")]
		[Space]

		public UnityEvent OnLandEvent;

		public UnityEvent OnJumpEvent;
		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }

		public BoolEvent OnLedgeClimbEvent;


		private void Awake()
		{
			m_Rigidbody2D = GetComponent<Rigidbody2D>();

			if (OnLandEvent == null)
				OnLandEvent = new UnityEvent();

			if (OnJumpEvent == null)
				OnJumpEvent = new UnityEvent();

			if (OnLedgeClimbEvent == null)
				OnLedgeClimbEvent = new BoolEvent();

			//xPos = transform.position.x;


		}



		private void FixedUpdate()
		{
			bool wasGrounded = isGrounded;
			isGrounded = false;

			isGrounded = myFeet.IsTouchingLayers(whatIsGround);
			//isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);


			if (isFacingRight)
			{
				isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, reachDistance, whatIsGround);
				isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, reachDistance, whatIsGround);
				isTouchingButton = Physics2D.Raycast(ledgeCheck.position, transform.right, reachDistance, whatIsButton);
			}
			else
			{
				isTouchingWall = Physics2D.Raycast(wallCheck.position, -transform.right, reachDistance, whatIsGround);
				isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, -transform.right, reachDistance, whatIsGround);
				isTouchingButton = Physics2D.Raycast(ledgeCheck.position, -transform.right, reachDistance, whatIsButton);
			}

			if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
			{
				ledgeDetected = true;
				ledgePosBot = wallCheck.position;
			}

			if (isGrounded)
			{
				OnLandEvent.Invoke();
			}
			else
			{
				OnJumpEvent.Invoke();
			}


		}

		public void Move(float moveForce, Vector2 jumpVector, GameObject validObstacle)
		{

			if (isGrounded && canMove)
			{
				Vector3 targetVelocity = new Vector2(moveForce * 10f, m_Rigidbody2D.velocity.y);
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
				if (moveForce > 0 && !isFacingRight && canFlip)
				{
					Flip();
				}
				else if (moveForce < 0 && isFacingRight && canFlip)
				{
					Flip();
				}

			}
			if (isGrounded && jumpVector != Vector2.zero)
			{
				isGrounded = false;

				m_Rigidbody2D.AddForce(jumpVector);

			}
			if (isGrounded && validObstacle != null)
			{
				canFlip = false;
				objectCatched = validObstacle;
				objectCatched.GetComponent<FixedJoint2D>().enabled = true;
				objectCatched.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
				//m_Grounded = false;


			}
			else if (isGrounded && validObstacle == null && canMove)
			{
				if (objectCatched != null)
				{
					objectCatched.GetComponent<FixedJoint2D>().enabled = false;
					objectCatched = null;
					canFlip = true;
				}

			}

			if (!isGrounded && canMove)
			{

				if (ledgeDetected && !canClimbLedge)
				{
					canClimbLedge = true;

					if (isFacingRight)
					{
						ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + reachDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
						ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + reachDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
					}
					else
					{
						ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - reachDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
						ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - reachDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
					}

					//TODO: Find a way to stop the 
					canMove = false;
					canFlip = false;
					m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
					//m_Rigidbody2D.isKinematic = true;

					OnLedgeClimbEvent.Invoke(canClimbLedge);
					//anim.SetBool("canClimbLedge", canClimbLed ge);
				}

				if (canClimbLedge)
				{
					transform.position = ledgePos1;
				}

			}


		}



		private void Flip()
		{

			// Switch the way the player is labelled as facing.
			isFacingRight = !isFacingRight;

			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;

		}


		public void FinishLedgeClimb()
		{
			//print("Reached FinishLedgeClimb");
			canClimbLedge = false;
			transform.position = ledgePos2;
			canMove = true;
			canFlip = true;

			ledgeDetected = false;
			m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
			OnLedgeClimbEvent.Invoke(canClimbLedge);
			//anim.SetBool("canClimbLedge", canClimbLedge);
		}

		/// <summary>
		/// Teleport to some position.
		/// </summary>
		/// <param name="position"></param>
		public void Teleport(Vector3 position)
		{
			m_Rigidbody2D.position = position;
			velocity *= 0;
			m_Rigidbody2D.velocity *= 0;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			//Gizmos.DrawLine(transform.position,
			//	(Vector2)transform.position + Vector2.right * transform.localScale.x * reachDistance);

			Gizmos.DrawLine(ledgeCheck.position,
				(Vector2)ledgeCheck.position + Vector2.right * transform.localScale.x * reachDistance);

			Gizmos.DrawLine(wallCheck.position,
				(Vector2)wallCheck.position + Vector2.right * transform.localScale.x * reachDistance);


			//Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

			//Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
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

}
