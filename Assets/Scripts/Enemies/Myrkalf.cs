using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myrkalf : Enemy
{
    // Myrkalf specific stats and logic
    public float projectileDamage;
    public float projSpeed;
    public GameObject projectilePrefab;
    public float fleeRange;
    private Vector3 fleeDirection;
    private bool hitCollidable = false;
    private float defaultMoveSpeed;

    // Projectile spawn and direction related
    public Transform firePoint;
    private GameObject arrowProjectile;
    private Vector3 targetLastPos;
    private Vector3 direction;

    protected override void Start()
    {
        base.Start();
        setDamage(projectileDamage); //set damage for projectilePrefab
        defaultMoveSpeed = moveSpeed;
        attackCooldown = 1.2f;

    }
    protected override void FixedUpdate()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsTag("attack") && isAlive && !hitCollidable && Vector2.Distance(transform.position, target.position) < fleeRange)
        {
            flee();
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            isFleeing = false;
            base.FixedUpdate();
        }
      

    }
  
    private void flee()
    {
        isFleeing = true;
        moveSpeed = 2.5f;
        fleeDirection = (transform.position - target.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, fleeDirection + transform.position, moveSpeed * Time.deltaTime); //move towards opposite direction of the player
        animator.SetBool("running", true);
        facePlayer();
        if(spriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = false;
        }
        else 
        {
            spriteRenderer.flipX = true;
        }
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collidables") || other.gameObject.CompareTag("HighCollidables")) //stop the charge attack if boar hit a collidable
        {
            hitCollidable = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collidables") || other.gameObject.CompareTag("HighCollidables")) //stop the charge attack if boar hit a collidable
        {
            hitCollidable = false;

        }
    }
}
