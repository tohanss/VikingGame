using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myrkalf_boss : Enemy
{
    // Myrkalf specific stats and logic
    public float projectileDamage;
    public float homingprojectileDamage;
    public float projSpeed;
    public float fleeRange;
    private Vector3 fleeDirection;
    private bool hitCollidable = false;
    private float defaultMoveSpeed;
    private int homingCounter;
    [SerializeField]private int homingRate;
    // Projectile spawn and direction related
    public GameObject projectilePrefab;
    public GameObject homingPrefab;
    public Transform firePoint;
    private GameObject arrowProjectile;
    private GameObject homingProjectile;
    private Vector3 targetLastPos;
    private Vector3 direction;

    protected override void Start()
    {
        base.Start();
        setDamage(projectileDamage); //set damage for projectilePrefab
        defaultMoveSpeed = moveSpeed;
        attackCooldown = 1.2f;
        homingCounter = 0;
    }
    protected override void FixedUpdate()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("attack") && isAlive && !hitCollidable && Vector2.Distance(transform.position, target.position) < fleeRange)
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
    //Myrkalf will flee if player gets to close and start attacking/moving when player gets outside its fleeRange
    private void flee()
    {
        isFleeing = true;
        moveSpeed = 3f;
        fleeDirection = (transform.position - target.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, fleeDirection + transform.position, moveSpeed * Time.deltaTime); //move towards opposite direction of the player
        animator.SetBool("running", true);
        facePlayer();
        if (spriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    //the myrkalf boss shots three consecutive arrows instead of one
    protected override void attack()
    {
        rotateFirepoint();
        targetLastPos = target.position;
        direction = (targetLastPos - firePoint.position) + new Vector3(0,0.5f,0);
        arrowProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        arrowProjectile.GetComponent<Rigidbody2D>().velocity = direction.normalized * projSpeed;
        arrowProjectile.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        lastTime = Time.time;
        homingCounter++;
        animator.ResetTrigger("attack");

        //if cooldown has passed, shoot two homing arrows
        if (homingCounter >= homingRate*3)
        {
            float dotRight = Vector3.Dot(Vector3.right, direction.normalized);
            float x;
            float y;

            if (dotRight > 0.5f || dotRight < -0.5)
            {
                print("The player is on the sides");
                x = 0;
                y = 5f;
            }
            else
            {
                Debug.Log("The player is on Y");
                x = 5f;
                y = 0;
            }

            homingProjectile = Instantiate(homingPrefab, firePoint.position, firePoint.rotation);
            homingProjectile.GetComponent<Myrkalf_homingProj>().startingXOffset = x;
            homingProjectile.GetComponent<Myrkalf_homingProj>().startingYOffset = y;

            homingProjectile = Instantiate(homingPrefab, firePoint.position, firePoint.rotation);
            homingProjectile.GetComponent<Myrkalf_homingProj>().startingXOffset = -x;
            homingProjectile.GetComponent<Myrkalf_homingProj>().startingYOffset = -y;

            homingCounter = 0;
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
    private void setDamage(float projectileDamage)
    {
        projectilePrefab.GetComponent<Myrkalf_projectile>().setDamage(projectileDamage);
        homingPrefab.GetComponent<Myrkalf_homingProj>().setDamage(homingprojectileDamage);
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
        if (other.gameObject.CompareTag("Collidables") || other.gameObject.CompareTag("HighCollidables")) //stop flee when the myrkalf hits a collidable
        {
            hitCollidable = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collidables") || other.gameObject.CompareTag("HighCollidables")) //reset boolean so myrkalf can flee again
        {
            hitCollidable = false;

        }
    }
}
