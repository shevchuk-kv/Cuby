using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Entity
{
    Rigidbody2D rigidBody;
    int direction;
    Collider2D[] colliders;

    private void Awake()
    {
        direction = -1;
        rigidBody = GetComponent<Rigidbody2D>();
        CalculateStatWeight();

        SetWalkBehaviour(new EnemyBehaviour(rigidBody, this.speed));
        rigidBody.mass = weight;
    }

    private void FixedUpdate()
    {
        //Поиск пересечений с объектом перед собой
        colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5f + transform.right * direction * 0.7f, 0.1f);

        if (colliders.Length > 0 && colliders.All(x => !x.GetComponent<PlayerController>())) 
            direction *= -1;

        //Поиск пересечений с полом перед собой
        colliders = Physics2D.OverlapCircleAll(transform.position + transform.right * direction - transform.up * 0.5f, 0.1f);

        if (colliders.Length == 0)
            direction *= -1;

        PerformMove((float)direction);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("ChangeHealth", -1);            
        }
    }
}
