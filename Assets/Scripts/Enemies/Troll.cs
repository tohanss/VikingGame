using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Enemy
{
    //Troll specific stats
    private float slamDamage;
    // Misc
    private Vector3 targetLastPos;
    private Vector3 direction;

    protected override void Start()
    {
        base.Start();
        attackCooldown = 2.0f;
    }
    protected override void attack()
    {
        if (!isAttacking)
        {
            targetLastPos = target.position; 
            direction = (targetLastPos - transform.position).normalized;
            lastAttackStartTime = Time.time;

        }
        if (Time.time - lastTime > attackCooldown)
        {
            isAttacking = true;
        }
        else 
        {
            isAttacking = false;

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isAttacking)
        {
            if (!other.GetComponent<PlayerActions>().isInvulnerable) //only do damage if player is not invulnerable
            {
                other.GetComponent<PlayerActions>().playerTakeDamage(slamDamage);
            }
        }
    }

}
