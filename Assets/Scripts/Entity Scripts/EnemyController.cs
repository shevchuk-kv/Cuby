using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Entity
{
    Rigidbody2D body;
    int direction;
    Collider2D[] colliders;

    void Start()
    {
        speed = 70f;
        direction = -1;
        body = GetComponent<Rigidbody2D>();
        SetWalkBehaviour(new EnemyBehaviour(body, this.speed));
    }
    private void FixedUpdate()
    {
        //Поиск пересечений с объектом перед собой
        colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5f + transform.right * direction * 0.7f, 0.1f);

        if (colliders.Length > 0) 
            direction *= -1;

        //Поиск пересечений с полом перед собой
        colliders = Physics2D.OverlapCircleAll(transform.position + transform.right * direction - transform.up * 0.5f, 0.1f);

        if (colliders.Length == 0)
            direction *= -1;

        PerformMove((float)direction);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        /*PlayerController player = other.GetComponent<PlayerController>();

        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 0.3f)
            player.ChangeHealth(-1);
            */

        Entity player = other.GetComponent<Entity>();

        if (player && player is PlayerController)
        {
            player.ChangeHealth(-1);
        }
    }
}
