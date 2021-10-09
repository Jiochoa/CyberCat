using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// elevator using YouTube tutorial
/// https://www.youtube.com/watch?v=N_3MV07QELY
/// </summary>

public class Elevator : MonoBehaviour
{
    public Transform player;
    public Transform elevatorSwitch;
    public Transform upperPos;
    public Transform downPos;
    public SpriteRenderer elevatorRenderer;

    public float speed;
    bool isElevatorDown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartElevator();
        DisplayColor();
    }


    void StartElevator()
    {
        if(Vector2.Distance(player.position, elevatorSwitch.position) < 0.5f  && Input.GetKeyDown(KeyCode.E))
        {
            print("E detected");
            if(transform.position.y >= downPos.position.y)
            {
                isElevatorDown = true;
            }
            else if (transform.position.y <= upperPos.position.y)
            {
                isElevatorDown = false;
            }
  
            
            
        }

        if(!isElevatorDown)
        {
            transform.position = Vector2.MoveTowards(transform.position, downPos.position, speed * Time.deltaTime);
            
        } 
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, upperPos.position, speed * Time.deltaTime);

        }
    }

    void DisplayColor()
    {
        if (transform.position.y >= downPos.position.y || transform.position.y <= upperPos.position.y)
        {
            elevatorRenderer.color = Color.green;
        } 
        else
        {
            elevatorRenderer.color = Color.red;
        }
    }

}
