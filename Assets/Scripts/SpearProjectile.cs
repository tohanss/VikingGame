using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearProjectile : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    void Start()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        
        rb.velocity = direction.normalized * speed;
    }
}
