using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearProjectile : MonoBehaviour
{
    // Stats, initilized by prefab
    public float damage;

    // Logic
    public Rigidbody2D rb;
    private Collider2D hitEnemy;
    private bool hit = false;

    // Life related
    private float maxLife = 1.0f; //temporary solution, max life duration for projectile 
    private float lifeTimer;

    // Upgrade related
    public bool doubleDamageInSpearRange = false;
    public bool pierce = false;

    public bool attachCrow;
    public GameObject crowDotPrefab;
    [SerializeField] private GameObject poof;

    private void Start()
    {
        StartCoroutine(destructOnTime( maxLife ));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !(hitEnemy == other))
        {
            hitEnemy = other;

            if (!hit && attachCrow)
                Instantiate(crowDotPrefab, other.transform);

            //Knockback and damage
            if (!hit)
            {
                if (other.GetComponent<Enemy>().canBeKnockedBack)
                {
                    Vector2 knockBackDir = GetComponent<Rigidbody2D>().velocity.normalized; //direction of the projectile
                    other.GetComponent<Enemy>().knockback(knockBackDir);
                }
                if (doubleDamageInSpearRange && (transform.position - GameObject.FindWithTag("Player").transform.position).magnitude < 2)
                {
                    hit = true;
                    other.GetComponent<Enemy>().takeDamage(damage * 2);
                }
                else
                {
                    hit = true;
                    other.GetComponent<Enemy>().takeDamage(damage);
                }

            }

            if (!pierce)
            {
                destruct();
            }
            else
            {
                hit = false;
            }
        }
        else if(other.gameObject.CompareTag("HighCollidables"))
        {
            destruct();
        }
    }

    public void setDamage(float value)
    {
        damage = value;
        crowDotPrefab.GetComponent<CrowDotEffect>().tickDamage = value * 0.1f;
    }

    private void destruct()
    {
        Instantiate(poof, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    IEnumerator destructOnTime(float time)
    {
        yield return new WaitForSeconds(time);
        destruct();
    }
}
