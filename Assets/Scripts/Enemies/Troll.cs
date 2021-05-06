    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Enemy
{
    //Troll specific stats
    public float slamDamage;
    public GameObject slamPrefab;
    private TrollSlamAttack trollSlamAttack;

    // Misc
    private ParticleSystem particleSystem;
    public Transform attackPoint;

    protected override void Start()
    {
        base.Start();
        particleSystem = slamPrefab.GetComponent<ParticleSystem>();
        var shape = particleSystem.shape;
        shape.radius = attackRange;

        trollSlamAttack = slamPrefab.GetComponent<TrollSlamAttack>();

        attackCooldown = 3.0f;
        setDamage(slamDamage); //set damage for slamPrefab
    }

    protected override void attack()
    {
        if (spriteRenderer.flipX)
        {
            attackPoint.localPosition = Vector2.right;
        }
        else
        {
            attackPoint.localPosition = Vector2.left;
        }

        //if cooldown has passed, do slam attack
        if (Time.time - lastTime > attackCooldown)
        {
            Instantiate(slamPrefab, attackPoint.position, attackPoint.rotation);
            lastTime = Time.time;
            animator.ResetTrigger("attack");
        }
    }

    private void setDamage(float slamDamage)
    {
        trollSlamAttack.setDamage(slamDamage);
    }


}
