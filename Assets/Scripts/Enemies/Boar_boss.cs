using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar_boss : Enemy
{
    //Boar specific stats
    public float chargeSpeed;
    public float chargeDuration; //how long to keep charging
    public float chargeDamage;

    // Misc
    private Vector3 targetLastPos;
    private Vector3 direction;
    private Collider2D hitPlayer;
    private bool hitCollidable = false;

    //Orb related
    public GameObject orbPrefab;
    public float orbDamage;

    protected override void Start()
    {
        base.Start();
        attackCooldown = 2.0f;
        setDamage(orbDamage);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
       
    }
    protected override void attack()
    {
        if (!isAttacking)
        {
            if (health <= maxHealth*0.7)
            {
                transform.GetChild(4).gameObject.SetActive(true);
            }
            targetLastPos = target.position;
            direction = (targetLastPos - transform.position).normalized;
            lastAttackStartTime = Time.time;

        }
        //if cooldown has passed, do charge attack in the direction of the player when the attack started

        isAttacking = true;
        enemyRigidbody.velocity = direction * chargeSpeed;

        //Stop charge attack when a certain ammount of time has passed
        if (Time.time - lastAttackStartTime >= chargeDuration || hitCollidable)
        {
            animator.Play("idle");
            enemyRigidbody.velocity = Vector2.zero;
            lastTime = Time.time;
            isAttacking = false;
            hitCollidable = false;
            hitPlayer = null;
        }
    }

    protected override void move()
    {
        if (aggravated || (Vector2.Distance(transform.position, target.position) <= aggroRange))
        {
            aggravated = true; //always aggravated if you have entered the myrkalf aggro range
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            animator.SetBool("running", true);
        }
        else animator.SetBool("running", false);

    }
    //Handles player taking damage and player can't get hit more than once per attack
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isAttacking && !(hitPlayer == other))
        {
            hitPlayer = other;
            if (!other.GetComponent<PlayerActions>().isInvulnerable) //only do damage if player is not invulnerable
            {
                other.GetComponent<PlayerActions>().playerTakeDamage(chargeDamage);
            }

        }
        else if (other.gameObject.CompareTag("Collidables") || other.gameObject.CompareTag("HighCollidables")) //stop the charge attack if boar hit a collidable
        {
            hitCollidable = true;
        }
    }

    private void setDamage(float orbDamange)
    {
        orbPrefab.GetComponent<Orbit>().setDamage(orbDamange);

    }
}
