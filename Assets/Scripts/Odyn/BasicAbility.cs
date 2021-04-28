using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAbility : MonoBehaviour
{
    // Projectile spawn and direction related
    public Transform firePoint;
    private Vector2 direction;
    private bool lookRight;

    // Basic attack
    public GameObject projectilePrefab;
    private SpearProjectile projectileScript;
    public float delay = 0.4f;
    private float timeSinceJump = 0f;
    private bool attacking = false;

    private PlayerMovement playerMovement;
    private Animator animator;

    // Upgrade related
    public bool basicPierce;
    public int numberProjectiles;

    // Attack stats
    private float scatterMaxAngle = 30;
    private float projectileSpeed = 10f;

    public bool spedUP = false;

    void Start()
    {
        projectileScript = projectilePrefab.GetComponent<SpearProjectile>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        // Apply pierce upgrade to prefab
        setPierce(basicPierce);

        // PLACEHOLDER TO SHOW HOW ATTACK SPEED CAN BE UPGRADED IN THE FUTURE
        if (spedUP)
        {
            animator.SetFloat("SpeedMultiplier", 2);
            delay = delay / 2;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !attacking)
        {
            animator.SetTrigger("Basic");

            // Set one of three spawn points for projectile depending on mouse position.
            Vector3 cameraPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (cameraPoint.x > transform.position.x)
            {
                // look right, dont flip sprite
                lookRight = true;
                playerMovement.spriteRenderer.flipX = false;

                if (cameraPoint.y < transform.position.y)
                {
                    direction = cameraPoint - (firePoint.position + new Vector3(1, -0.5f, 0));
                }
            }
            else
            {
                // Look left and flip sprite
                lookRight = false;
                playerMovement.spriteRenderer.flipX = true;

                if (cameraPoint.y < transform.position.y)
                {
                    direction = cameraPoint - (firePoint.position + new Vector3(-1, -0.5f, 0));
                }
            }

            if (cameraPoint.y > transform.position.y) 
            {
                direction = cameraPoint - firePoint.position;
            }

            attacking = true;
            playerMovement.moveable = false;
        }
    }

    private void FixedUpdate()
    {
        if (attacking)
        {
            timeSinceJump += Time.fixedDeltaTime;
            if (timeSinceJump > delay)
            {
                Attack();
                attacking = false;
                playerMovement.moveable = true;
                timeSinceJump = 0f;
            }
        }
    }

    // Attack Related
    private void Attack()
    {   
        Vector3 offset;
        if (direction.y < 0)
        {
            if (lookRight)
            {
                offset = new Vector3(1f, -0.5f, 0);
            } 
            else
            {
                offset = new Vector3(-1f, -0.5f, 0);
            }
        }
        else
        {
            offset = new Vector3(direction.x, direction.y, 0).normalized;
        } 

        // Set start rotation
        direction = Quaternion.AngleAxis(scatterMaxAngle, Vector3.forward) * direction;

        // Set rotation appropriate for number of projectiles
        float rotationStep = (scatterMaxAngle*2)/(numberProjectiles+1);

        for (int i = 0; i < numberProjectiles; i++)
        {   
            Rigidbody2D rb = Instantiate(projectilePrefab, firePoint.position + offset, firePoint.rotation).GetComponent<Rigidbody2D>();        
            
            // modify direction to spread out projectiles
            direction = Quaternion.AngleAxis(-rotationStep, Vector3.forward) * direction;

            rb.velocity = direction.normalized * projectileSpeed;
            rb.transform.right = rb.velocity;
            rb.transform.rotation = rb.transform.rotation * Quaternion.Euler( new Vector3(0, 0, -45));
        }
    }

    public void setDamage(int damage)
    {
        projectileScript.damage = damage;
    }

    public void setPierce(bool value)
    {
        projectileScript.pierce = value;
    }
}