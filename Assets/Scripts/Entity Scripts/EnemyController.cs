using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Entity
{
    Rigidbody2D rigidBody;
    int direction;
    float kSize;
    Collider2D[] colliders;

    private void Awake()
    {
        direction = -1;
        speed *= 0.5f;
        rigidBody = GetComponent<Rigidbody2D>();
        CalculateStatWeight();
        kSize = size > minSize + (maxSize - minSize * 0.5) ? 1.50f : 1;

        SetWalkBehaviour(new EnemyBehaviour(rigidBody, this.speed));
        rigidBody.mass = weight;
    }

    private void FixedUpdate()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position + transform.right * 0.55f * direction * kSize, 0.05f);
        if (colliders.Length > 0 && colliders.All(x => !x.GetComponent<PlayerController>())) 
            direction *= -1;



        Debug.DrawLine(transform.position, transform.position + transform.right * 0.55f * direction * kSize);

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
