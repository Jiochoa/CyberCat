using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------
//Adds a switch to the Mover object to give it the functionality of an
//elevator
//--------------------------------------------------------------------

/*TODO: 
* Bug: only works on the first go
* Fix: it sohuld only be added once every time it reaches an end
* 
*/
public class Elevator : MonoBehaviour
{
    [Header("Elevator")]
    [SerializeField] Mover moverObject;
    [SerializeField] MeshRenderer platform;
    [SerializeField] Transform player;
    [SerializeField] Transform elevatorSwitch;
    [SerializeField] Transform upperPos;
    [SerializeField] Transform downPos;
    public SpriteRenderer elevatorRenderer;


    bool elevatorIsUp = false;
    bool elevatorIsDown = false;
    bool eleLock = true;
    private void Start()
    {
        downPos.position = new Vector2(downPos.position.x, downPos.position.y - 20);
    }

    // Update is called once per frame
    void Update()
    {
        bool playerCanReachSwitch = Vector2.Distance(player.position, elevatorSwitch.position) < 0.5f;
        bool elevatorButtonPressed = Input.GetKeyDown(KeyCode.Q);
            // Pre: player is within reach 
        /*
            if (platfromIsEnabled && playerCanReachSwitch)
            {
                DisplayColor(); // Green
                if (elevatorButtonPressed)
                {
                    platfromIsEnabled = false;
                    DisplayColor(); // Red
                    //movePlatform();
                }
            }
        }
        */

        UpdatePlatformPosition();

        /*TODO: 
         * Bug: position is adding 20 more than once
         * Fix: it sohuld only be added once every time it reaches an end
         * 
         * 
         * 
         */


        if(elevatorIsUp && eleLock)
        {
            moverObject.DisablePlatform();
            upperPos.position = new Vector2(upperPos.position.x, upperPos.position.y + 20);
            downPos.position = new Vector2(upperPos.position.x, upperPos.position.y + 20);
            eleLock = false;
        }

        if (elevatorIsDown && eleLock)
        {
            moverObject.DisablePlatform();
            upperPos.position = new Vector2(upperPos.position.x, upperPos.position.y - 20);
            downPos.position = new Vector2(upperPos.position.x, upperPos.position.y - 20);
            eleLock = false;
        }

        if(playerCanReachSwitch && elevatorButtonPressed)
        {
            moverObject.EnablePlatform();
            eleLock = true;
        }


       




    }

    void UpdatePlatformPosition()
    {
        if(Vector2.Distance(platform.transform.position, upperPos.position) < 0.5f)
        {
            elevatorIsUp = true;
            UpdateColor();
            print("elevatorIsUp = " + elevatorIsUp);
        } 
        else if (Vector2.Distance(platform.transform.position, downPos.position) < 0.5f)
        {
            elevatorIsDown = true;
            UpdateColor();
            print("elevatorIsDown = " + elevatorIsDown);
        } 
        else
        {
            elevatorIsUp = false;
            elevatorIsDown = false;
            UpdateColor();
        }


    }



    void UpdateColor()
    {
        if (moverObject.platfromIsEnabled)
        {
            elevatorRenderer.color = Color.green;
        }
        else
        {
            elevatorRenderer.color = Color.red;
        }
    }

}
