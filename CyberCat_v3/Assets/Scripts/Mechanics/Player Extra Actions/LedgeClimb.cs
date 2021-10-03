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
        private bool isTouchingLedge;
        private bool ledgeDetected;



        private void Awake()
        {
            playerKO = GetComponent<KinematicObject>();

        }

        private void FixedUpdate()
        {
            /*
            if (playerKO.IsFacingRight)
            {
                //isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, reachDistance, whatIsGround);
                //isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, reachDistance, whatIsGround);
                //isTouchingButton = Physics2D.Raycast(ledgeCheck.position, transform.right, reachDistance, whatIsButton);
            }
            else
            {
                //isTouchingWall = Physics2D.Raycast(wallCheck.position, -transform.right, reachDistance, whatIsGround);
                //isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, -transform.right, reachDistance, whatIsGround);
                //isTouchingButton = Physics2D.Raycast(ledgeCheck.position, -transform.right, reachDistance, whatIsButton);
            }

            if (playerKO.IsTouchingWall && !isTouchingLedge && !ledgeDetected)
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
            */

        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

