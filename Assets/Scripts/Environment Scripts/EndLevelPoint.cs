using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelPoint : MonoBehaviour
{
    bool levelComplete = false;
    public bool LevelComplete { get => levelComplete; set => levelComplete = value; }
    void OnTriggerEnter2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();

        if (entity && entity is PlayerController)
        {
            levelComplete = true;
        }
    }
}
