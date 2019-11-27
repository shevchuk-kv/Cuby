using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entity : MonoBehaviour
{
    protected float speed = 150.0f;

    protected float size = 1.0f;
    [SerializeField]
    protected float maxSize = 1.0f;
    [SerializeField]
    protected float minSize = 0.5f;

    protected int currentHealth = 1;
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    [SerializeField]
    protected int maxHealth = 3;
    [SerializeField]
    protected int minHealth = 1;

    protected int weight = 1;
    [SerializeField]
    protected int maxWeight = 5;
    [SerializeField]
    protected int minWeight = 1;

    IEntityBehaviour entityBehaviour;

    public void SetWalkBehaviour(IEntityBehaviour behaviour) => entityBehaviour = behaviour;

    public void PerformMove(float direction) => entityBehaviour.Move(direction);

    public virtual void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        if (currentHealth == 0)
            Debug.Log("Died");
    }
}


