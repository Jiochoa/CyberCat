using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Tell the player when to move and at what speed
    /// </summary>
    public class PMovement : MonoBehaviour
    {
        [SerializeField] private LayerMask whatIsObstacle;


        // Catche
        public PController controller;
        public LineRenderer lineRenderer;
        public Animator animator;


        // Player input Commands
        private string inputHorizontal = "Horizontal";
        private string inputVertical = "Vertical";
        private string inputHoldJump = "Hold Jump";
        private string inputJump = "Jump";
        private string inputAction = "Action";

        // Config
        public float runSpeed = 10f;
        public float shortJumpForce = 400f;
        public float longJumpForce = 500f;

        // Temp variables
        private float horizontalMove = 0f;
        private Vector2 jumpVector;
        private bool actionMove = false;

        //private GameObject objectDetected;
        private bool canCatchInput = true;
        public float reachDistance = 1f;
        GameObject objectDetected;
        RaycastHit2D playerReach;


        Camera myCamera;
        Vector2 force;
        Vector3 startPoint;
        Vector3 endPoint;
        //public float power = 10f;
        private Vector2 minPower = new Vector2(-8,-8); // TODO: Why 8?
        private Vector2 maxPower = new Vector2(8, 8);

        private void Start()
        {
            myCamera = Camera.main;
            lineRenderer = GetComponent<LineRenderer>();

        }
        private void Update()
        {
            PlayerMove();
            PlayerJump();
            PlayerAction();
        }

        private void FixedUpdate()
        {
            controller.Move(
                horizontalMove * Time.fixedDeltaTime,
                jumpVector,
                objectDetected);

            horizontalMove = 0;
            jumpVector = Vector2.zero;
            //actionMove = false;

        }
    
        private void PlayerMove()
        {
            //bool horizontalButtonPressed = Input.GetButton(inputHorizontal);
            //if (horizontalButtonPressed && canCatchInput)
            if (canCatchInput)
            {
                horizontalMove = Input.GetAxis(inputHorizontal) * runSpeed;
                animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
                print("horizontalMove = " + horizontalMove);

                if (animator.GetBool("isGrabbing") == true)
                {
                    animator.SetFloat("Pushing", (horizontalMove));
                }
            }

        }
   
        private void PlayerJump()
        {

            // SHORT JUMP
            bool shortJumpPressed = Input.GetButtonDown(inputJump);
            bool longJumpPressed = Input.GetButtonDown(inputHoldJump);
            if (shortJumpPressed && !longJumpPressed && canCatchInput)
            {
                jumpVector = new Vector2(0f, shortJumpForce);
                //animator.SetBool("isJumping", true);
            }


            // LONG JUMP
            bool mouseButtonIsPressed = Input.GetMouseButtonDown(0);
            if (mouseButtonIsPressed && canCatchInput)
            {
                canCatchInput = false;
                startPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
                startPoint.z = 15f;
                animator.SetFloat("Speed", 0f);

                if (!animator.GetBool("isCharging"))
                {
                    animator.SetBool("isCharging", true);

                }

            }

            // calculate long jump
            bool mouseButtonIsHeldDown = Input.GetMouseButton(0);
            if (mouseButtonIsHeldDown)
            {
                Vector3 curentPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
                curentPoint.z = 15;
                StartLongJumpLine(startPoint, curentPoint);
                animator.SetBool("isCharging", true);


            }

            // finish calculating long jump
            bool mouseButtonIsReleased = Input.GetMouseButtonUp(0);
            if (mouseButtonIsReleased)
            {
                endPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
                startPoint.z = 15f;

                force = new Vector2(
                    Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
                    Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));

                jumpVector = force * longJumpForce;
                EndLongJumpLine();
                canCatchInput = true;
                animator.SetBool("isCharging", false);

                //animator.SetBool("isJumping", true);

            }





        }
    
        private void PlayerAction()
        {
            //Physics2D.queriesStartInColliders = false;
            playerReach = Physics2D.Raycast(
                transform.position,
                Vector2.right * transform.localScale.x,
                reachDistance,
                whatIsObstacle);

            // check is valid obstacle is within characters reach
            bool actionButtonPressed = Input.GetButtonDown(inputAction);
            bool actionButtonReleased = Input.GetButtonUp(inputAction);
            if (actionButtonPressed && playerReach.collider != null
                && playerReach.collider.tag == "Movable")
            {
                // send box object to controller
                animator.SetBool("isGrabbing", true);
                objectDetected = playerReach.collider.gameObject;
            }
            else if (actionButtonReleased)
            {
                animator.SetBool("isGrabbing", false);
                objectDetected = null;
            }


        }
  
        public void StartLongJumpLine(Vector3 startPoint, Vector3 endPoint)
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

        public void OnLanding()
        {
            animator.SetBool("isJumping", false);
        }

        public void OnJumping()
        {
            animator.SetBool("isJumping", true);
        }

        public void OnLedgeClimbing(bool climbing)
        {
            animator.SetBool("canClimbLedge", climbing);
        }



    }

}


