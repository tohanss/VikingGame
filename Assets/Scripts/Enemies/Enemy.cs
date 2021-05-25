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
    protected float health;

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
    [Tooltip("This parameter doesn't change attack range for the troll")]
    public float attackRange;
    [HideInInspector]
    public bool aggravated = false; //true if taken damage
    protected bool isAttacking = false;
    public float approachRange; //how close the enemy will move when attack is on cooldown
    // Material related
    protected Material matWhite;
    protected Material matDeath;
    protected Material matDefault;
    protected SpriteRenderer spriteRenderer;

    // Misc
    protected Rigidbody2D enemyRigidbody;
    public GameObject damageNumbers;
    protected Animator animator;
    private float knockBackForce = 10f;
    public HealthBar hpBar;
    [HideInInspector]
    public bool isAlive;
    [HideInInspector]
    public bool isFleeing;

    // Sound
    public AudioClip takeDamageSound;
    public AudioClip deathSound;
    [HideInInspector]
    public AudioSource audioSource;

    protected virtual void Start()
    {
        health = maxHealth;
        canBeKnockedBack = true;
        isAlive = true;
        isFleeing = false;
        enemyRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        matWhite = Resources.Load("Materials/White-Flash", typeof(Material)) as Material;
        matDeath = Resources.Load("Materials/Dissolve", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;

        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        target = playerCharacter.GetComponent<Transform>();
        hpBar.SetMaxHealth(health); //, maxHealth);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isAttacking && isAlive && !isFleeing) 
        {
            facePlayer();
        }
    }

    protected virtual void FixedUpdate()
    {
        animator.SetBool("running", false);
        if (Vector2.Distance(transform.position, target.position) > approachRange && !animator.GetCurrentAnimatorStateInfo(0).IsTag("attack") && isAlive)
        {
            move();
        }
        if (!isAttacking && Vector2.Distance(transform.position, target.position) <= attackRange && Time.time - lastTime > attackCooldown && isAlive)
        {
            animator.SetTrigger("attack"); //attack is triggered by animation
        } 
        if (isAttacking && isAlive) //Continues attack (Boar)
        {
            animator.ResetTrigger("attack");
            attack();
        }
        if (!isAlive) //stand still when dying
        {
            animator.Play("idle");
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

    protected virtual void facePlayer() 
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
        if (isAlive)
        {
            // play damage sound
            audioSource.pitch = Random.Range(0.90f, 1.1f);
            audioSource.PlayOneShot(takeDamageSound, 0.25f);
                
            GameObject damageNumber = Instantiate(damageNumbers, transform.position, Quaternion.identity);
            damageNumber.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();

            aggravated = true;
            health -= damage;
            spriteRenderer.material = matWhite;
            hpBar.SetHealth(health);
            // hpBar.SetHealth(health, maxHealth);
            if (health <= 0)
            {
                audioSource.PlayOneShot(deathSound, 0.6f);
                isAlive = false;
                GetComponent<BoxCollider2D>().enabled = false;
                enemyRigidbody.velocity = Vector2.zero;
                StartCoroutine(die());
            }
            else
            {
                Invoke("resetMat", 0.1f);
            }
        }
        
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
        enemyRigidbody.AddForce(force, ForceMode2D.Impulse);
        canBeKnockedBack = false;
        StartCoroutine(knockbackTimer());
    }

    //Resets the material to default
    private void resetMat() 
    {
        if(isAlive)
        spriteRenderer.material = matDefault;
    }

    protected virtual IEnumerator die()
    {
        transform.parent.transform.parent.GetComponent<RoomManager>().decrementNumberOfEnemies();
        playerCharacter.GetComponent<PlayerActions>().GainExp(expValue);
        spriteRenderer.material = matDeath;

        Transform light;
        if (light = gameObject.transform.Find("Point Light 2D")){

            light.gameObject.SetActive(false);
        }

        float ticks = 10f;
        for (int i = 1; i < ticks+1; i++)
        {
            spriteRenderer.material.SetFloat("_Fade", 1-i/ticks);
            yield return new WaitForSeconds(0.08f);
        }
        Destroy(gameObject);
    }
}
