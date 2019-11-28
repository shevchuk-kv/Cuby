using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    float jumpForce = 42.0f;
    Rigidbody2D rigidBody;

    bool onGround = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    
    private void Awake()
    {
        CalculateStatWeight();
        jumpForce *= size * size;
    }

    void Start()
    {
        HealthBar.instance.SetValue(currentHealth, maxHealth);

        rigidBody = GetComponent<Rigidbody2D>();
        SetWalkBehaviour(new PlayerBehaviour(rigidBody, this.speed));
        rigidBody.mass = weight;
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
    }

    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        PerformMove(horizontalMove);

        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

    }

    public override void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        HealthBar.instance.SetValue(currentHealth, maxHealth);               
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var direction = transform.InverseTransformPoint(collision.transform.position);
            rigidBody.AddForce(direction * -9.0f * weight, ForceMode2D.Impulse);
        }
    }

}


