using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class AirConditioner : MonoBehaviour
    {
        public Transform player;
        public Transform fanSwitch;
        public SpriteRenderer switchRenderer;
        public AreaEffector2D areaOfEffect;

        bool isBlowingIn = true;
        public float fanPower = 1f;


        // Start is called before the first frame update
        void Start() 
        {
            areaOfEffect = GetComponent<AreaEffector2D>();
        }

        // Update is called once per frame
        void Update()
        {
            StartAirConditioner();
            DisplayColor();
        }

        void StartAirConditioner()
        {
            if (Vector2.Distance(player.position, fanSwitch.position) < 0.5f && Input.GetKeyDown(KeyCode.E))
            {
                fanPower *= -1;
                areaOfEffect.forceMagnitude = fanPower;

                if (isBlowingIn) {
                    isBlowingIn = false; 
                } else
                {
                    isBlowingIn = true;
                }

            }
        }


        /*
        void OnTriggerEnter2D(Collider2D other)
        {
            var rb = other.attachedRigidbody;
            if (rb == null) return;
            var player = rb.GetComponent<PlayerController>();
            if (player == null) return;
            AddVelocity(player);

            //rb.AddRelativeForce(fanPower);
        }

        void AddVelocity(PlayerController player)
        {

            player.Move(fanPower) ;
            
        }

        */

        void DisplayColor()
        {
            if (!isBlowingIn)
            {
                switchRenderer.color = Color.green;
            }
            else
            {
                switchRenderer.color = Color.red;
            }
        }

    }

}

