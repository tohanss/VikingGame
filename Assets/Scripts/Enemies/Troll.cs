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
    private ParticleSystem ps;
    public Transform attackPoint;
    private CircleCollider2D slamCollider;
    public Transform shadow;

    protected override void Start()
    {
        base.Start();
        ps = slamPrefab.GetComponent<ParticleSystem>();
        trollSlamAttack = slamPrefab.GetComponent<TrollSlamAttack>();
        slamCollider = slamPrefab.GetComponent<CircleCollider2D>();
        
        var shape = ps.shape;
        attackRange = shape.radius;
        slamCollider.radius = attackRange;
        attackCooldown = 3.0f;
        setDamage(slamDamage); //set damage for slamPrefab
        audioSource = GetComponent<AudioSource>();
    }

    protected override void facePlayer()
    {
        base.facePlayer();
        if (spriteRenderer.flipX)
        {
            shadow.localPosition = new Vector2(-0.03f, 0);
        }
        else
        {
            shadow.localPosition = new Vector2(0.05f, 0);
        }
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
            audioSource.Play();
            lastTime = Time.time;
            animator.ResetTrigger("attack");
        }
    }

    private void setDamage(float slamDamage)
    {
        trollSlamAttack.setDamage(slamDamage);
    }


}
