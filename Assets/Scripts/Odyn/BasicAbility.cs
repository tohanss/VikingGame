using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private PlayerActions playerAction;
    private Animator animator;
    private Image icon;

    // Upgrade related
    public bool pierce;
    public bool doubleDamageInSpearRange;
    public bool attachCrow;
    public int numberProjectiles = 1;
    private float attackSpeedMultiplier = 1f; 

    // Attack stats
    private float scatterMaxAngle = 30;
    private float projectileSpeed = 10f;

    //Sound
    public AudioClip attackSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        projectileScript = projectilePrefab.GetComponent<SpearProjectile>();
        animator = GetComponent<Animator>();
        playerAction = GetComponent<PlayerActions>();
        icon = playerAction.canvas.transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<Image>();

        // Apply pierce upgrade to prefab
        setPierce(pierce);
        setDoubleDamageInSpearRange(doubleDamageInSpearRange);
        setCrowDotEffect(attachCrow);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !attacking && !playerAction.isActive)
        {
            animator.SetTrigger("Basic");

            audioSource.pitch = Random.Range(1f, 1.2f);
            audioSource.PlayOneShot(attackSound, 8f);

            icon.color = new Color(1, 1, 1, 0.3f);
            Invoke("resetIcon", 0.15f);
            playerAction.isActive = true;
            // Set one of three spawn points for projectile depending on mouse position.
            Vector3 cameraPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (cameraPoint.x > transform.position.x)
            {
                // look right, dont flip sprite
                lookRight = true;
                playerAction.spriteRenderer.flipX = false;

                if (cameraPoint.y < transform.position.y)
                {
                    direction = cameraPoint - (firePoint.position + new Vector3(1, -0.5f, 0));
                }
            }
            else
            {
                // Look left and flip sprite
                lookRight = false;
                playerAction.spriteRenderer.flipX = true;

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
            playerAction.moveable = false;
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
                playerAction.moveable = true;
                playerAction.isActive = false;
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

    private void resetIcon()
    {
        icon.color = Color.white;
    }

    public void setDamage(int damage)
    {
        projectileScript.setDamage(damage);
    }

    public void setPierce(bool value)
    {
        pierce = value;
        projectileScript.pierce = value;
    }

    public void setCrowDotEffect(bool value)
    {
        attachCrow = value;
        projectileScript.attachCrow = value;
    }

    public void setDoubleDamageInSpearRange(bool value)
    {
        doubleDamageInSpearRange = value;
        projectileScript.doubleDamageInSpearRange = value;
    }

    public void increaseAttackSpeed(float multiplier)
    {       
        attackSpeedMultiplier *= multiplier;
        delay /= multiplier;
        animator.SetFloat("SpeedMultiplier", attackSpeedMultiplier);
    }
}