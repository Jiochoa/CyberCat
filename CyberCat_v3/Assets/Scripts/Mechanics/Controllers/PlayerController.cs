using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using System;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        // -- CATCHE --
        [Header("Audio Settings")]
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public Joystick movementJoystick;
        public Joystick actionJoystick;

        // -- ACTIONS --
        private string BUTTON_HORIZONTAL = "Horizontal";
        private string BUTTON_JUMP = "Jump";
        // private string ActionButton = "Action";

        // Max horizontal speed of the player.
        public float maxSpeed = 7;

        // Initial jump velocity at the start of a jump.
        public float jumpTakeOffSpeed = 10;
        bool jump;
        private bool stopJump;
        public float holdjumpTakeOffSpeed = 9;
        public JumpState jumpState = JumpState.Grounded;

        // Ledge Climb
        public LedgeState ledgeState = LedgeState.NoLedgeCloseBy;

        // Player's movement vars
        Vector2 move;
        public bool controlEnabled = true;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        Vector3 theScale;


        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            //joystick = GetComponent<Joystick>();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                // get Move intput 
                move.x = movementJoystick.Horizontal + Input.GetAxis(BUTTON_HORIZONTAL);

                // Get Jump input
                
                if (jumpState == JumpState.Grounded && (Input.GetButtonDown(BUTTON_JUMP) || actionJoystick.Vertical > 0.5))
                {
                    jumpState = JumpState.PrepareToJump;
                    //controlEnabled = false;
                }
                else if (Input.GetButtonUp(BUTTON_JUMP) )
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            //UpdateLedgeState();
            base.Update();
        }


        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                // ready to jump -> initiate jumping
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                // Start jump -> stay in the air
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                // In the air -> reaches the ground
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    } else
                    {
                        //check for ledge to climb
                        //print("This is whe we update ledge climb state");
                    }
                    break;
                // Reached the ground -> ready to jump again
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    //controlEnabled = true;
                    break;
            }
        }

        //TODO: Ledge Climb mechanics
        private void UpdateLedgeState()
        {
            switch (ledgeState)
            {
                // in the air -> able to ledge detectcted
                // able to ledge climb -> ledge detected
                case LedgeState.PrepateToLedgeClimb:

                    break;
                // ledge detected -> position player in the edge
                //case LedgeState.
                // player in ledge position -> start ledge climb
                // climbing ledge -> over the ledge  


            }
        }
        protected override void ComputeVelocity()
        {
            
            if (jump && IsGrounded)
            {
                // new velocity of entity with added jump force
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    // velocity of entity while falling
                   velocity.y = velocity.y * model.jumpDeceleration;
                }
            }
            
            // flip right
            if (move.x > 0.01f)
            {
                spriteRenderer.flipX = false;

                IsFacingRight = true;
            }
            // flip left
            else if (move.x < -0.01f)
            {
                spriteRenderer.flipX = true;

                IsFacingRight = false;

            }

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        public enum LedgeState
        {
            PrepateToLedgeClimb,
            NoLedgeCloseBy,
            LedgeDetected,
            ClimbingLedge
        }
    }
}