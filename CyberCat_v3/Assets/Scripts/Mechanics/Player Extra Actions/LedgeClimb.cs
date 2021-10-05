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
    public class LedgeClimb : MonoBehaviour
    {
        // -- CATCHE --
        private KinematicObject playerKO;

        // -- SETUP --
        [Header("Ledge Climb Settings")]
        [SerializeField] private Transform ledgeCheck;
        [SerializeField] private LayerMask whatIsGround;


        public float reachDistance = 0.5f;
        private bool isTouchingLedge;
        private bool ledgeDetected;

        private bool canClimbLedge = false;
        private Vector2 ledgePosBot;
        private Vector2 ledgePos1;
        private Vector2 ledgePos2;
        public float ledgeClimbXOffset1 = 0f;
        public float ledgeClimbYOffset1 = 0f;
        public float ledgeClimbXOffset2 = 0f;
        public float ledgeClimbYOffset2 = 0f;

        private void Awake()
        {
            playerKO = GetComponent<KinematicObject>();

        }

        void Update()
        {
            if (playerKO.IsFacingRight)
            {
                isTouchingLedge = Physics2D.Raycast(
                    ledgeCheck.position, playerKO.transform.right, reachDistance, 
                    whatIsGround);
            }
            else
            {
                isTouchingLedge = Physics2D.Raycast(
                    ledgeCheck.position, -playerKO.transform.right, reachDistance, 
                    whatIsGround);
            }
            
            if (playerKO.IsTouchingWall && !isTouchingLedge && !ledgeDetected)
            {
                print("canClimbLedge");
                ledgeDetected = true;
                ledgePosBot = playerKO.transform.position;
                //ledgePosBot = wallCheck.position;

            }


        }
        
        public void CheckLedgeClimb()
        {
            if (ledgeDetected && !canClimbLedge)
            {
                canClimbLedge = true;

                if (playerKO.IsFacingRight)
                {
                    ledgePos1 = new Vector2(
                        Mathf.Floor(ledgePosBot.x + reachDistance) - ledgeClimbXOffset1, 
                        Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                    ledgePos2 = new Vector2(
                        Mathf.Floor(ledgePosBot.x + reachDistance) + ledgeClimbXOffset2, 
                        Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
                }
                else
                {
                    ledgePos1 = new Vector2(
                        Mathf.Ceil(ledgePosBot.x - reachDistance) + ledgeClimbXOffset1, 
                        Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                    ledgePos2 = new Vector2(
                        Mathf.Ceil(ledgePosBot.x - reachDistance) - ledgeClimbXOffset2, 
                        Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
                }

                //canMove = false;
                //canFlip = false;
                //m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                playerKO.body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;


                //OnLedgeClimbEvent.Invoke(canClimbLedge);
                //anim.SetBool("canClimbLedge", canClimbLedge);
            }

            if (canClimbLedge)
            {
                transform.position = ledgePos1;
            }

        }

        public void FinishLedgeClimb()
        {
            print("Reached FinishLedgeClimb");
            canClimbLedge = false;
            transform.position = ledgePos2;
            //canMove = true;
            //canFlip = true;

            ledgeDetected = false;
            playerKO.body.constraints = RigidbodyConstraints2D.FreezeRotation;
            //OnLedgeClimbEvent.Invoke(canClimbLedge);
            //anim.SetBool("canClimbLedge", canClimbLedge);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            // ground check sphere
            Gizmos.DrawLine(
                ledgeCheck.position,
                (Vector2)ledgeCheck.position + Vector2.right * playerKO.transform.localScale.x * reachDistance
                );
            
            //Gizmos.DrawSphere(transform.position, shellRadius + 1);
            // wall check ray

            // ledge check ray




        }

    }
}

