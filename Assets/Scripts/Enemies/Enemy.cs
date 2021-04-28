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
    private void Start()
    {
        health = maxHealth;
        spriteRenderer = transform.GetComponent<SpriteRenderer>();

        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        target = playerCharacter.GetComponent<Transform>();
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
        if (Vector2.Distance(transform.position, target.position) > attackRange)
        {
            move();
        }
        if (Vector2.Distance(transform.position, target.position) <= attackRange || isAttacking)
        {
            attack();
        }
    }

    protected virtual void move()
    {   //move towards player if taken damage or within aggro range
        if (aggravated || (Vector2.Distance(transform.position, target.position) <= aggroRange)) 
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

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


}
