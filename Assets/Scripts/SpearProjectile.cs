using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearProjectile : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float damage = 5f;

    private float maxLife = 4.0f; //temporary solution, max life duration for projectile 
    private float lifeTimer;

    void Start()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        
        rb.velocity = direction.normalized * speed;
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLife) 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().takeDamage(damage);
            knockback(other);
            Destroy(gameObject);

        }
        else 
        {
            Destroy(gameObject);
        }

    }
    //Knockback the the hit object
    private void knockback(Collider2D other) 
    {
        Vector2 difference = other.transform.position - transform.position;
        other.transform.position = new Vector2((other.transform.position.x + difference.x), (other.transform.position.y + difference.y));
    }
}
