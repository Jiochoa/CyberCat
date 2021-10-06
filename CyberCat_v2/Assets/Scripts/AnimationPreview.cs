using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace DeveloperTools.AnimationPreview
{
    /// <summary>
    /// Preview animations of an Animator inside the Unity Editor
    /// 
    /// Usage:
    /// 
    ///   + create empty gameobject and attach this script to it
    ///   + drag a gameobject with an animator from the scene into the Animator slot
    ///   + use the control functions to play animations
    /// 
    /// </summary>
    public class AnimationPreview : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField]
        public int clipIndex = -1;

        [SerializeField]
        public string clipName = "";

        [SerializeField]
        public Animator animator;
#endif
    }
}