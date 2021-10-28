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

        UpdatePlatformPosition();


        if(elevatorIsUp && eleLock)
        {
            moverObject.DisablePlatform();
            upperPos.position = new Vector2(upperPos.position.x, upperPos.position.y + 20);
            downPos.position = new Vector2(downPos.position.x, downPos.position.y + 20);
            eleLock = false;
        }

        if (elevatorIsDown && eleLock)
        {
            moverObject.DisablePlatform();
            upperPos.position = new Vector2(upperPos.position.x, upperPos.position.y - 20);
            downPos.position = new Vector2(downPos.position.x, downPos.position.y - 20);
            eleLock = false;
        }

        if(playerCanReachSwitch && elevatorButtonPressed && (elevatorIsUp || elevatorIsDown))
        {
            moverObject.EnablePlatform();
            eleLock = true;
            print("player pressed button succesfully");
        }


       




    }

    void UpdatePlatformPosition()
    {


        if(Vector2.Distance(platform.transform.position, moverObject.upperBound.Normalize) < 0.5f)
        {
            elevatorIsUp = true;
            elevatorIsDown = false;
            UpdateColor();
            print("elevatorIsUp = " + elevatorIsUp);
        } 
        else if (Vector2.Distance(platform.transform.position, downPos.position) < 0.5f)
        {
            elevatorIsUp = false;
            elevatorIsDown = true;
            UpdateColor();
            print("elevatorIsDown = " + elevatorIsDown);
        } 
        else
        {
            elevatorIsUp = false;
            elevatorIsDown = false;
            UpdateColor();
            print("elevator is moving");
        }


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

    [SerializeField] Mover platform1;
    [SerializeField] Transform player1;
    [SerializeField] Transform elevatorSwitch1;
    [SerializeField] Transform higherPosition1;
    [SerializeField] Transform lowerPosition1;
    public SpriteRenderer elevatorRenderer1;

    MeshRenderer platformMesh;

    void Start1()
    {
        platformMesh = platform1.GetComponent<MeshRenderer>();
    }

    void Update1()
    {
        bool playerPressedSwitch = false;
        bool playerCanReachSwitch = false;
        bool platformIsUp = false;
        bool platformIsDown = false;

        //moverObject.GetComponent<MeshRenderer>();
       

        if(playerCanReachSwitch && playerPressedSwitch)
        {

        }

    }
    //Check where is the platform
    void UpdatePlatformPosition1()
    {
        /*  if platform is in its highest position
         *      platformIsUp = true;    
         *  if platfomr is in its lowest position
         *      platformIsDown = true;
         *  if neither
         *      platformIsUp = false;    
         *      platformIsDown = false;
         */






    }




}
