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
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;

        // -- ACTIONS --
        private string horizButton = "Horizontal";
        private string JumpButton = "Jump";
        // private string ActionButton = "Action";

        // Max horizontal speed of the player.
        public float maxSpeed = 7;

        // Initial jump velocity at the start of a jump.
        public float jumpTakeOffSpeed = 7;
        public float holdjumpTakeOffSpeed = 9;
        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;

        // Ledge Climb
        public LedgeState ledgeState = LedgeState.NoLedgeCloseBy;

        // Player's movement vars
        //public Health health;
        public bool controlEnabled = true;
        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            //health = GetComponent<Health>();
            //audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            //animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis(horizButton);
                if (jumpState == JumpState.Grounded && Input.GetButtonDown(JumpButton))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp(JumpButton))
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
            UpdateLedgeState();
            base.Update();
        }

       

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                // jump button pressed -> initiate jumping
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
                    }
                    break;
                // Reached the ground -> ready to jump again
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }
        private void UpdateLedgeState()
        {
            switch (ledgeState)
            {
                // in the air -> able to ledge detectcted
                // able to ledge climb -> ledge detected
                // ledge detected -> position player in the edge
                // player in ledge position -> start ledge climb
                // climbing ledge -> over the ledge 
            }
        }
        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            //animator.SetBool("grounded", IsGrounded);
            //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

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
            NoLedgeCloseBy,
            LedgeDetected,
            ClimbingLedge
        }
    }
}