using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    //Boar specific stats
    public float chargeSpeed;
    public float chargeDuration; //how long to keep charging
    private float chargeDamage = 8f;

    // Misc
    private Vector3 targetLastPos;
    private Vector3 direction;
    private Collider2D hitPlayer;

    protected override void attack()
    {
        if (!isAttacking)
        {
            targetLastPos = target.position; // This so boar doesen't target feet rather targets the center of player
            direction = (targetLastPos - transform.position).normalized;
            lastAttackStartTime = Time.time;

        }
        //if cooldown has passed, do charge attack in the direction of the player when the attack started
        if (Time.time - lastTime > attackCooldown)
        {
            isAttacking = true;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + direction, chargeSpeed * Time.deltaTime);       

        }
        //Stop charge attack when a certain ammount of time has passed
        if (Time.time - lastAttackStartTime >= chargeDuration)
        {
            lastTime = Time.time;
            isAttacking = false;
            hitPlayer = null;
        }
    }
    //Handles player taking damage and player can't get hit more than once per attack
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.gameObject.CompareTag("Player") && isAttacking && !(hitPlayer == other))
        {
            hitPlayer = other;
            other.GetComponent<PlayerActions>().playerTakeDamage(chargeDamage);
        }
    }

}
