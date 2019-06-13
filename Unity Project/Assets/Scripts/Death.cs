using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public GameObject player;

   void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet") {
            print("hitbox!");
            Destroy(player.gameObject);
        }
    }
}
