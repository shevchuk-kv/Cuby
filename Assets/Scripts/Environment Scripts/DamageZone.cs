using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entity;
public class DamageZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.ChangeHealth(-controller.Health);
        }
    }
}
