using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myrkalf : Enemy
{
    // Myrkalf specific stats and logic
    public float projectileDamage;
    public float projSpeed;
    public GameObject projectilePrefab;
    private Myrkalf_projectile myrkalf_Projectile;

    // Projectile spawn and direction related
    public Transform firePoint;
    private GameObject arrowProjectile;
    private Vector3 targetLastPos;
    private Vector3 direction;

    protected override void Start()
    {
        base.Start();
        myrkalf_Projectile = projectilePrefab.GetComponent<Myrkalf_projectile>();
        setDamage(projectileDamage); //set damage for projectilePrefab
        attackCooldown = 1.2f;

    }

    protected override void move()
    {
        base.move();
    }

    protected override void attack()
    {
        rotateFirepoint();
        targetLastPos = target.position;
        direction = (targetLastPos - transform.position);

        //if cooldown has passed, shoot an arrow
        if (Time.time - lastTime > attackCooldown)
        {
            arrowProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            arrowProjectile.GetComponent<Rigidbody2D>().velocity = direction.normalized * projSpeed;
            arrowProjectile.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            lastTime = Time.time;
            animator.ResetTrigger("attack");
        }
    }
    private void setDamage(float projectileDamage)
    {
        projectilePrefab.GetComponent<Myrkalf_projectile>().setDamage(projectileDamage);
    }

    private void rotateFirepoint()
    {
        if (spriteRenderer.flipX)
        {
            firePoint.localPosition = new Vector2(0.54f, 0.69f);
        }
        else
        {
            firePoint.localPosition = new Vector2(-0.54f, 0.69f);
        }
    }
}
