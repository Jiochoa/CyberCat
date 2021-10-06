using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tell the player when to move and at what speed
/// </summary>
public class PlayerBehavior : MonoBehaviour
{
    //public PlayerController controller;
    public Animator animator;

    float horizontalMove = 0f;
    public float runSpeed = 10f;
    bool jump = false;
    bool action = false;


    // Moving Commands
    private string inputHorizontal = "Horizontal";
    private string inputJump = "Jump";
    private string inputAction = "Action";

 
    void Update()
    {
        // Player movement horizontal
        horizontalMove = Input.GetAxis(inputHorizontal) * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        
        // Buttons
        bool jumpButtonPressed = Input.GetButtonDown(inputJump) || Input.GetMouseButtonUp(0);
        bool actionButtonPressed = Input.GetButtonDown(inputAction); 
        

        // Player movement jump
        if (jumpButtonPressed)
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }

        // Player action (grab)
        if (actionButtonPressed)
        {
            action = true;
            // animator.SetBool("action", true);
        }

        
    }


    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    private void FixedUpdate()
    {
        // Move out character
        //controller.Move(horizontalMove * Time.fixedDeltaTime, jump, action);
        jump = false;
    }







}
