using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Entity player = other.GetComponent<Entity>();


        /*if (Mathf.Abs(player.transform.position.y - (transform.position.y + transform.up.y)) < 0.3f)
        {
            player.ChangeHealth(-1);
        }*/

        if (player && player is PlayerController)
        {
            player.ChangeHealth(-player.CurrentHealth);            
        }

    }
}
