using UnityEngine;
using System;
using System.Collections;

public class FloatBehavior : MonoBehaviour
{
    float originalY;

    public float floatStrength = 1; 
    public bool start; 

    void Start()
    {
        this.originalY = this.transform.position.y;
    }

    void Update()
    {
        
        float old_y = transform.position.y; 
        float new_y =  originalY + ((float)Math.Sin(Time.time * 1.2f) * floatStrength);

        if (Mathf.Abs(new_y - old_y) < 0.1f) { start = true; }

        if (start) { 
            transform.position = new Vector2(transform.position.x, originalY + ((float)Math.Sin(Time.time * 1.2) * floatStrength));
        }
    }
}