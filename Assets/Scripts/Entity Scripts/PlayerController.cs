using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity
{
    public class PlayerController : Entity
    {
        float jumpForce = 12.0f;
        Rigidbody2D body;
 
        bool onGround = false;
        public Transform groundCheck;
        float groundRadius = 0.2f;
        public LayerMask whatIsGround;

        public int Health { get { return currentHealth; } }

        bool isInvincible;
        float invincibleTimer;
        float timeInvincible = 2.0f;
        void Start()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            onGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        }

        void Update()
        {
            float horizontalMove = Input.GetAxis("Horizontal");
            body.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, body.velocity.y);

            if (onGround && Input.GetKeyDown(KeyCode.Space))
            {
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0)
                    isInvincible = false;
            }
        }

        public void ChangeHealth(int amount)
        {
            if (amount < 0)
            {
                if (isInvincible)
                    return;

                isInvincible = true;
                invincibleTimer = timeInvincible;
            }

            currentHealth = Mathf.Clamp(currentHealth + amount, minHealth, maxHealth);

            Debug.Log(currentHealth + "/" + maxHealth);
        }


    }
}

