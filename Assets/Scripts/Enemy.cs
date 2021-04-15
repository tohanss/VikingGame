using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float moveSpeed;
    public float maxHealth;
    private float health;

    public Transform target;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (health <= 0)
        {
            die();
        }
    }

    private void move()
    {
        
    }

    private void attack() 
    { 

    }

    void die() 
    {
        Destroy(gameObject);
    }

    public void takeDamage(float damage) 
    {
        health -= damage;
    }
}
