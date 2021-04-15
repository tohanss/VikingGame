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

    public GameObject damageNumbers;

    private void Start()
    {
        health = maxHealth;
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    private void Update()
    {
        //Enemy faces player by fliping the sprite
        if (Vector2.Distance(transform.position, target.position) <= aggroRange && transform.position.x < target.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if(Vector2.Distance(transform.position, target.position) <= aggroRange)
        {
            spriteRenderer.flipX = false;
        }


    }

    private void FixedUpdate()
    {
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
        GameObject damageNumber = Instantiate(damageNumbers, transform.position, Quaternion.identity);
        damageNumber.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();

        health -= damage;
                
        if (health <= 0)
        {
            die();
        }
    }

}
