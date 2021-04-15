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
    public float aggroRange;
    public float attackRange;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        health = maxHealth;
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    private void Update()
    {
        
        if (health <= 0)
        {
            die();
        }

        //Enemy faces player by fliping the sprite
        if (transform.position.x < target.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        move();

    }

    private void move()
    {
        if (Vector2.Distance(transform.position, target.position) <= aggroRange && Vector2.Distance(transform.position, target.position) > attackRange) 
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

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
