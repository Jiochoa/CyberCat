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

    private bool enableSwitch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool playerCanReachSwitch = Vector2.Distance(player.position, elevatorSwitch.position) < 0.5f;
        bool elevatorButtonPressed = Input.GetKeyDown(KeyCode.E);

        // Pre: player is within reach and player pressed the switch
        if (enableSwitch && playerCanReachSwitch)
        {
            
            /*  if platform is not moving and elevator button is pressed
             *  
             *      disable switch and color it red
             *      
             *      move platform to next destination
             *      
             *  else platform is moving
             *  
             *      do nothing
             */

            if(elevatorButtonPressed && platformNotMoving)
            {

            }



        }
    }

    void DisplayColor()
    {
        if (enableSwitch)
        {
            elevatorRenderer.color = Color.green;
        }
        else
        {
            elevatorRenderer.color = Color.red;
        }
    }

}
