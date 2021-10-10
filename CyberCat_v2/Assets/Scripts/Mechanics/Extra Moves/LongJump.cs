using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class LongJump : MonoBehaviour
    {
        [SerializeField] public Joystick joystick;
        public Animator animator;
        LineRenderer lr;
        Camera myCamera;
        Rigidbody2D rb;

        public float power = 10f;
        public float maxDrag = 5f;
        Vector3 startPoint;
        Vector3 endPoint;

        Vector3 dragStartPos;
        //Touch touch;
        bool bool1 = false;
        bool bool2 = false;

        private void Start()
        {
            myCamera = Camera.main;

            rb = GetComponent<Rigidbody2D>();
            lr = GetComponent<LineRenderer>();

        }

        // Update is called once per frame
        void Update()
        {

            //if(touch.phase == TouchPhase.Began)
            bool detectedJoystick = joystick.Vertical < -0.1f;
                //(Mathf.Abs(joystick.Vertical) > joystick.DeadZone 
                //|| Mathf.Abs(joystick.Vertical) > joystick.DeadZone);

            // 0 & 0 & 0 => noInput & 0 & 0

            if (detectedJoystick && bool1 == false && bool2 == false) // 1 && 0 && 0
            {
                DragStart();
                bool2 = true;
                print("started Long Jump");

                startPoint = myCamera.ScreenToWorldPoint(joystick.handle.transform.position);// Input.mousePosition);
                startPoint.z = 15f;
                animator.SetFloat("Speed", 0f);
            }



            //if(touch.phase == TouchPhase.Moved)
            if (detectedJoystick && bool1 == false && bool2 == true)  // 1 && 0 && 1
            {
                Dragging();
                bool1 = true;
                print("calculating Long Jump");

                Vector3 curentPoint = myCamera.ScreenToWorldPoint(joystick.handle.transform.position);//Input.mousePosition);
                curentPoint.z = 15;
                StartLongJumpLine(startPoint, curentPoint);
                animator.SetBool("isCharging", true);
            }
                
            //bool isDoneCalculatingLongJump = !isReadyToLongJump;

            //if(touch.phase == TouchPhase.Ended)
            if (!detectedJoystick && bool1 == true && bool2 == true) // 0 && 1 && 1
            {
                DragRelease();
                bool1 = false;
                bool2 = false;
                print("ended Long Jump");

                endPoint = myCamera.ScreenToWorldPoint(joystick.transform.position);//Input.mousePosition);
                startPoint.z = 15f;
                EndLongJumpLine();
                animator.SetBool("isCharging", false);
            }
       
        }




        void DragStart()
        {
            dragStartPos = Camera.main.ScreenToViewportPoint(joystick.handle.transform.position);
            dragStartPos.z = 0f;
            lr.positionCount = 1;
            lr.SetPosition(0, dragStartPos);
        }

        void Dragging()
        {
            Vector3 draggingPos = Camera.main.ScreenToViewportPoint(joystick.handle.transform.position);
            draggingPos.z = 0f;
            lr.positionCount = 2;
            lr.SetPosition(1, draggingPos);
        }

        void DragRelease()
        {
            lr.positionCount = 0;

            Vector3 dragReleasePos = Camera.main.ScreenToViewportPoint(joystick.handle.transform.position);
            dragReleasePos.z = 0f;

            Vector3 force = dragStartPos - dragReleasePos;
            Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;

            rb.AddForce(clampedForce, ForceMode2D.Impulse);
        }


        public void StartLongJumpLine(Vector3 startPoint, Vector3 endPoint)
        {
            lr.positionCount = 2;
            Vector3[] points = new Vector3[2];
            points[0] = startPoint;
            points[1] = endPoint;

            lr.SetPositions(points);
        }
        public void EndLongJumpLine()
        {
            lr.positionCount = 0;
        }

    }
}

