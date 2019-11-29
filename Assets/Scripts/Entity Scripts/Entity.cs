using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    protected float speed = 180.0f;

    protected float size = 1.0f;
    [SerializeField]
    protected float maxSize = 1.0f;
    [SerializeField]
    protected float minSize = 0.5f;

    protected int currentHealth;
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

    protected void CalculateStatWeight()
    {
        //UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        size = (float)Math.Round(UnityEngine.Random.Range(minSize, maxSize), 1);
        
        float kSize = (size - minSize) * 10 / ((maxSize - minSize) * 10);

        weight = (int)Math.Round(kSize == 0 ? minWeight : kSize * (maxWeight - minWeight + 1));

        currentHealth = (int)Math.Round(((double)weight / (maxWeight - minWeight + 1)) * (maxHealth - minHealth + 1));
        maxHealth = currentHealth;

        speed /= Mathf.Round(weight * 0.5f / size);        

        transform.localScale = new Vector3(size, size, 1);

        //Debug.LogFormat($"{size} | {kSize} | {weight} | {currentHealth}");
    }
}


