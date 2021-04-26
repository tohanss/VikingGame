using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAbility : MonoBehaviour
{
    // Basic attack
    public Transform firePoint;
    public GameObject projectilePrefab;
    private SpearProjectile projectileScript;
    private Vector2 direction;
    public float delay = 0.4f;
    private float timeSinceJump = 0f;
    private bool attacking = false;

    private PlayerMovement playerMovement;
    private Animator animator;

    // Upgrade related
    public bool basicPierce;
    public int numberProjectiles = 1;

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
        if (Input.GetButtonDown("Fire1")){
            animator.SetTrigger("Basic");
            direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position);
            attacking = true;
        }
    }

    private void FixedUpdate()
    {
        if (attacking)
        {
            timeSinceJump += Time.fixedDeltaTime;
            playerMovement.moveable = false;
            turnWhenAttack();
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
        // Set start rotation
        direction = Quaternion.AngleAxis(scatterMaxAngle, Vector3.forward) * direction;

        // Set rotation appropriate for number of projectiles
        float rotationStep = (scatterMaxAngle*2)/(numberProjectiles+1);

        for (int i = 0; i < numberProjectiles; i++)
        {   
            Vector3 spawnOffset = new Vector3(direction.x, direction.y, 0).normalized;
            Rigidbody2D rb = Instantiate(projectilePrefab, firePoint.position + spawnOffset, transform.rotation).GetComponent<Rigidbody2D>();        
            
            // modify direction to spread out projectiles
            direction = Quaternion.AngleAxis(-rotationStep, Vector3.forward) * direction;

            rb.velocity = direction.normalized * projectileSpeed;
            rb.transform.right = rb.velocity;
            rb.transform.rotation = rb.transform.rotation * Quaternion.Euler( new Vector3(0, 0, -45));
        }
    }
    
    //Makes player face the direction where you click attack
    private void turnWhenAttack() 
    {
        if (direction.x > 0)
        {
            playerMovement.spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            playerMovement.spriteRenderer.flipX = true;
        }
    }

    public void setDamage(int damage){
        projectileScript.damage = damage;
    }

    public void setPierce(bool value){
        projectileScript.pierce = value;
    }
}
