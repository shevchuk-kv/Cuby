using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    public int width = 30;
    public int height = 8;

    public Transform lava;

    void Start()
    {
        
        for (int x = -(width / 2); x < width; x++)
        {
            Instantiate(lava, new Vector3(x, -3, 0), Quaternion.identity);
        }
    }

    
}
