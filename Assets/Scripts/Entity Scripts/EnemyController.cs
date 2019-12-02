using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Entity
{
    Rigidbody2D rigidBody;
    int direction;
    float kSize;
    Collider2D[] colliders;
    LayerMask mask;
    private void Awake()
    {
        mask = LayerMask.GetMask("Ground");
        direction = -1;
        speed *= 0.5f;
        rigidBody = GetComponent<Rigidbody2D>();
        CalculateStatWeight();
        kSize = size > minSize + (maxSize - minSize * 0.5) ? 1.25f : size == 1 ? 1.8f : 1;

        SetWalkBehaviour(new EnemyBehaviour(rigidBody, this.speed));
        rigidBody.mass = weight;
    }

    private void FixedUpdate()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position + transform.right * 0.55f * direction * kSize, 0.05f);
        if (colliders.Length > 0 && colliders.All(x => !x.GetComponent<PlayerController>())) 
            direction *= -1;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down + Vector2.right * 0.35f * direction, 2f, mask.value);
        if (hit.collider == false)
            direction *= -1;

        Debug.DrawLine(transform.position, transform.position + transform.right * 0.55f * direction * kSize);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.6f + Vector3.right * 0.35f * direction);

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
