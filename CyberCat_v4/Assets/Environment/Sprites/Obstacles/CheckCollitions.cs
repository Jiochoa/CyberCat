using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollitions : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
    }
}
