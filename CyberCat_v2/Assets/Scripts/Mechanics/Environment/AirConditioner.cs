using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class AirConditioner : MonoBehaviour
    {
        //Transform player;
        Transform fanSwitch;
        SpriteRenderer switchRenderer;
        Transform areaOfEffect;

        bool isBlowingIn = false;
        public float fanPower = -1f;


        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update()
        {
            //StartAirConditioner();
            //DisplayColor();
        }

        void StartAirConditioner()
        {

        }

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

            player.Move(fanPower, Vector2.zero, null) ;
            
        }



        void DisplayColor()
        {
            if (isBlowingIn)
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

