using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Enemy stats
    public string enemyName;
    public float moveSpeed;
    public float maxHealth;
    public int expValue;
    private float health;

    // Cooldown related
    protected float lastTime = 0;
    protected float attackCooldown;
    protected float lastAttackStartTime;
    private float knockBackCooldown = 1f;
    [HideInInspector]
    public bool canBeKnockedBack;

    // reference to the player
    public GameObject playerCharacter;

    // Aggro related
    public Transform target;
    public float aggroRange;
    public float attackRange;
    private SpriteRenderer spriteRenderer;
    private bool aggravated = false; //true if taken damage
    protected bool isAttacking = false;

    // Misc
    public GameObject damageNumbers;
    private Animator animator;
    private float knockBackForce = 10f;
    public HealthBar hpBar;

    protected virtual void Start()
    {
        health = maxHealth;
        canBeKnockedBack = true;

        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();

        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        target = playerCharacter.GetComponent<Transform>();
        hpBar.SetMaxHealth(health); //, maxHealth);
    }

    private void Update()
    {
        if (!isAttacking) 
        {
            facePlayer();
        }

    }

    private void FixedUpdate()
    {
        animator.SetBool("running", false);
        if (Vector2.Distance(transform.position, target.position) > attackRange)
        {
            move();
        }
        if (!isAttacking && Vector2.Distance(transform.position, target.position) <= attackRange && Time.time - lastTime > attackCooldown)
        {
            animator.SetTrigger("attack");
            //attack is triggered by animation
        } 
        if (isAttacking)
        {
            animator.ResetTrigger("attack");
            attack();
        }
    }

    protected virtual void move()
    {   //move towards player if taken damage or within aggro range
        if (aggravated || (Vector2.Distance(transform.position, target.position) <= aggroRange))
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            animator.SetBool("running", true);
        }
        else animator.SetBool("running", false);

    }

    protected virtual void attack() 
    { 
        //does nothing now. For future cases.
    }

    protected void facePlayer() 
    {
        //Enemy faces player by fliping the sprite
        if ((aggravated || Vector2.Distance(transform.position, target.position) <= aggroRange) && transform.position.x < target.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (aggravated || (Vector2.Distance(transform.position, target.position) <= aggroRange))
        {
            spriteRenderer.flipX = false;
        }
    }

    public void takeDamage(float damage) 
    {
        GameObject damageNumber = Instantiate(damageNumbers, transform.position, Quaternion.identity);
        damageNumber.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();

        aggravated = true;
        health -= damage;
        hpBar.SetHealth(health);
        // hpBar.SetHealth(health, maxHealth);
        if (health <= 0)
        {
            die();
        }
    }

    protected virtual void die() 
    {
        playerCharacter.GetComponent<PlayerActions>().GainExp(expValue);
        Destroy(gameObject);
    }

    //handles cooldown for knockback
    public IEnumerator knockbackTimer()
    {
        yield return new WaitForSeconds(knockBackCooldown);
        canBeKnockedBack = true;
    }

    //Handles the knockback of this enemy gameObject
    public void knockback(Vector2 knockBackDirection)
    {
        Vector2 direction = knockBackDirection;
        Vector2 force = direction * knockBackForce;
        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        canBeKnockedBack = false;
        StartCoroutine(knockbackTimer());
    }

}
