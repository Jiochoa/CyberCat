using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------
//Adds a switch to the Mover object to give it the functionality of an
//elevator
//--------------------------------------------------------------------

public class Elevator : MonoBehaviour
{
    [Header("Elevator")]
    Mover moverObject;
    [SerializeField] Transform player;
    [SerializeField] Transform elevatorSwitch;
    [SerializeField] Transform higherPosition;
    [SerializeField] Transform lowerPosition;
    public SpriteRenderer elevatorRenderer;

    MeshRenderer platform;
    bool platformIsUp = false;
    bool platformIsDown = false;
    bool elevatorIsMoving = false;

    bool canPressSwitch = false;
    bool switchPressed = false;

    void Start()
    {
        moverObject = GetComponentInChildren<Mover>();
        platform = moverObject.GetComponentInChildren<MeshRenderer>();
        // offset the lower position so the elevator starts from the bottom
        lowerPosition.position = new Vector2(lowerPosition.position.x, lowerPosition.position.y - 20);
    }

    void Update()
    {
        UpdatePlatformPosition();
        UpdateColor();

        if (platformIsUp || platformIsDown) // not moving
        {      
            moverObject.DisablePlatform();
            // platform not moving
            if (canPressSwitch)
            {
                // player can reach button

                if (switchPressed)
                {
                    // player clicked button
                    //// movePositions() v
                    if (platformIsUp)
                    {
                        higherPosition.position = new Vector2(higherPosition.position.x, higherPosition.position.y + 20);
                        lowerPosition.position = new Vector2(lowerPosition.position.x, lowerPosition.position.y + 20);
                    } 
                    else if(platformIsDown)
                    {
                        higherPosition.position = new Vector2(higherPosition.position.x, higherPosition.position.y - 20);
                        lowerPosition.position = new Vector2(lowerPosition.position.x, lowerPosition.position.y - 20);
                    }

                    moverObject.EnablePlatform();

                }
            }

        }
        else // moving
        {
            Debug.Log("Platform should be moving...");
        }

    }
    //Check where is the platform
    void UpdatePlatformPosition()
    {
        platformIsUp = Vector2.Distance(platform.transform.position, higherPosition.position) < 0.5f;
        platformIsDown = Vector2.Distance(platform.transform.position, lowerPosition.position) < 0.5f;
        canPressSwitch = Vector2.Distance(player.position, elevatorSwitch.position) < 0.5f;
        switchPressed = Input.GetKeyDown(KeyCode.Q);
    }

    void UpdateColor()
    {
        if (moverObject.platfromIsEnabled)
        {
            elevatorRenderer.color = Color.red;
        }
        else
        {
            elevatorRenderer.color = Color.green ;
        }
    }



}
