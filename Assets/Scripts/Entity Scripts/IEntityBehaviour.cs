using UnityEngine;
public interface IEntityBehaviour
{
    void Move(float direction);
    float SetMoveSpeed(float speed);
}
