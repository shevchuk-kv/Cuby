using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity
{
    public class Entity : MonoBehaviour
    {
        protected float speed = 260.0f;

        protected float size = 1.0f;
        protected float maxSize = 1.0f;
        protected float minSize = 0.5f;

        protected int currentHealth = 1;
        public int maxHealth = 3;
        public int minHealth = 1;

        protected int weight = 1;
        public int maxWeight = 5;
        public int minWeight = 1;
    }
}

