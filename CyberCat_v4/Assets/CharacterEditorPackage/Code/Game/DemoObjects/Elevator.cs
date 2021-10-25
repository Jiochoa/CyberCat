using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------
//Adds a switch to the Mover object to give it the functionality of an
//elevator
//--------------------------------------------------------------------

public class Elevator : Mover
{
    [Header("Elevator")]
    Transform player;
    Transform elevatorSwitch;
    Transform upperPos;
    Transform downPos;
    public SpriteRenderer elevatorRenderer;


    bool elevatorIsUp = false;
    bool elevatorIsDown = false;



    // Update is called once per frame
    void Update()
    {

        checkPlatformPosition();

        bool playerCanReachSwitch = Vector2.Distance(player.position, elevatorSwitch.position) < 0.5f;
        bool elevatorButtonPressed = Input.GetKeyDown(KeyCode.E);
        

        // Pre: player is within reach 
        if (platfromIsEnabled && playerCanReachSwitch)
        {
            DisplayColor(); // Green

            if(elevatorButtonPressed)
            {
                platfromIsEnabled = false;
                DisplayColor(); // Red
                //movePlatform();
            } 
        }

        



    }

    void checkPlatformPosition()
    {
        if(Vector2.Distance(transform.position, upperPos.position) < 0.5f)
        {
            elevatorIsUp = true;
            elevatorIsDown = false;
        } 
        else if (Vector2.Distance(transform.position, downPos.position) < 0.5f)
        {
            elevatorIsUp = false;
            elevatorIsDown = true;
        }


    }



    void DisplayColor()
    {
        if (platfromIsEnabled)
        {
            elevatorRenderer.color = Color.green;
        }
        else
        {
            elevatorRenderer.color = Color.red;
        }
    }

}
