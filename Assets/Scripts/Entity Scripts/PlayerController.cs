using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    float jumpForce = 12.0f;
    Rigidbody2D rigidBody;

    bool onGround = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public int Health { get { return currentHealth; } }

    bool isInvincible;
    float invincibleTimer;
    float timeInvincible = 1.0f;


    void Start()
    {
        currentHealth = maxHealth;
        rigidBody = GetComponent<Rigidbody2D>();
        SetWalkBehaviour(new PlayerBehaviour(rigidBody, this.speed));
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

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }

    public override void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(transform.up * 5.0f, ForceMode2D.Impulse);

        Debug.Log(currentHealth + "/" + maxHealth);
        if (currentHealth == 0)
            Debug.Log("Died");
    }


}


