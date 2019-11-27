using UnityEngine;

public class EnemyBehaviour : IEntityBehaviour
{
    private float speed;
    Rigidbody2D rigidBody;

    public EnemyBehaviour(Rigidbody2D _rigidBody, float _speed)
    {
        rigidBody = _rigidBody;
        speed = _speed;
    }

    public float SetMoveSpeed(float speed) => this.speed = speed;

    public void Move(float direction)
    {
        rigidBody.velocity = new Vector2(direction * speed * Time.deltaTime, rigidBody.velocity.y);
    }
}
