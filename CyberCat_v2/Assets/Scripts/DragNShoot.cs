using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
    public float power = 10f;
    Rigidbody2D myRigidbody2D;
    TrajectoryLine tl;

    public Vector2 minPower;
    public Vector2 maxPower;

    Camera myCamera;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    private void Start()
    {
        myCamera = Camera.main;
        tl = GetComponent<TrajectoryLine>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15f;
        }

        if (Input.GetMouseButton(0))
        { 
            Vector3 curentPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
            curentPoint.z = 15;
            tl.RenderLine(startPoint, curentPoint);

        }


        if(Input.GetMouseButtonUp(0))
        {
            endPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15f;

            force = new Vector2(
                Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), 
                Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            myRigidbody2D.AddForce(force * power, ForceMode2D.Impulse);
            tl.EndLine();
        }
    }

}
