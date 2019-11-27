using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Entity player = other.GetComponent<Entity>();
        
        if (player && player is PlayerController)
        {
            player.ChangeHealth(-player.CurrentHealth);
            
        }
    }
}
