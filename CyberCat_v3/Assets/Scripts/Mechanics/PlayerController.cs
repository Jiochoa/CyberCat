using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        //public AudioClip jumpAudio;
        //public AudioClip respawnAudio;
        //public AudioClip ouchAudio;

        public float maxSpeed = 7;

        public float jumpTakeOffSpeed = 7;
    }

}
