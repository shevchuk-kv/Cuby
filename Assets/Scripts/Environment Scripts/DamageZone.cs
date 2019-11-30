using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        
        if (entity && entity is PlayerController)
        {
            entity.ChangeHealth(-entity.CurrentHealth);            
        }else if (entity && entity is EnemyController)
        {
            Destroy(entity);
        }
    }
}
