using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
Rigidbody2D rigidbody2d;
    
void Start()
{
    rigidbody2d = GetComponent<Rigidbody2D>();
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
