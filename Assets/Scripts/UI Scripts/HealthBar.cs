using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public static HealthBar instance { get; private set; }
    [SerializeField]
    Text health = null;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame

    public void SetValue(int value, int maxValue)
    {
        health.text = value.ToString() + " / " + maxValue.ToString();
    }
}
