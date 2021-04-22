using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearProjectile : MonoBehaviour
{
    // stats
    public float damage;

    // Logic
    public Rigidbody2D rb;
    private Collider2D hitEnemy;

    // Life related
    private float maxLife = 4.0f; //temporary solution, max life duration for projectile 
    private float lifeTimer;
    public bool pierce = false;

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
        if (other.gameObject.CompareTag("Enemy") && !(hitEnemy == other))
        {
            hitEnemy = other;
            other.GetComponent<Enemy>().takeDamage(damage);
            knockback(other);
            if (!pierce){
                Destroy(gameObject);
            }
        }
        else if(other.gameObject.CompareTag("Collidables"))
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
