using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerActions : MonoBehaviour
{
    // Connected objects
    private PlayerClass playerClass;
    private Rigidbody2D playerRB;
    private Animator animator;
    public GameObject damageNumbers;

    // Player stats
    private int level = 1;
    private float currentHealth;
    public float movespeed;

    // Experience and level related
    private int currentExp = 0;
    private int requiredExp = 10;
    public Text favorText;
    public Text levelText;

    // Hidden variables in inspector
    [HideInInspector]
    public bool isActive = false;
    [HideInInspector]
    public bool isInvulnerable = false;
    [HideInInspector]
    public bool moveable = true;
    [HideInInspector]
    public Vector2 movement;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;


    // Unity functions
    private void Start() 
    {
        favorText.text = "Favour: " + currentExp.ToString() + " / " + requiredExp.ToString();
        levelText.text = "Level: " + level.ToString();

        playerClass = GetComponent<PlayerClass>();
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        currentHealth = playerClass.maxHealth;
    }

    private void Update()
    {
        if (moveable)
            playerFaceDirection();
    }

    private void FixedUpdate()
    {
        move();
    }

    // Level and EXP related
    public void GainExp(int exp) 
    {
        currentExp += exp;
        if (currentExp >= requiredExp){
            LevelUp();
        }

        favorText.text = "Favour: " + currentExp.ToString() + " / " + requiredExp.ToString();
    }

    private void LevelUp()
    {
        //Modify stats
        level += 1;
        playerClass.increaseDamage(2);
        playerClass.dropUpgrade(level);

        //Modify Exp values
        currentExp -= requiredExp;
        requiredExp += level * 5;

        // Change level text
        levelText.text = "Level: " + level.ToString();
    }

    public void playerTakeDamage(float damage)
    {
        if (isInvulnerable) return;
        
        Debug.Log("Damage Taken: " + damage);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            die();
        }
    }

    public void playerRestoreHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth > playerClass.maxHealth)
        {
            currentHealth = playerClass.maxHealth;
        }

        TextMesh damageNumber = Instantiate(damageNumbers, transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<TextMesh>();
        damageNumber.color = Color.green;
        damageNumber.text = amount.ToString();
    }

    //temporary death, restarts level if you died
    private void die()
    {
        Debug.Log("You Died");
        Debug.Log("Restarting Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart to current scene

    }
    //Handles the players facing direction
    void playerFaceDirection()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); //when moving right movement.x = 1 and left movement.x = -1
        movement.y = Input.GetAxisRaw("Vertical");

        //Don't turn when in attack animation, even if you are eg. holding "D" (moving right)
        if (movement.x > 0 && !animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            spriteRenderer.flipX = false;

        }
        if (movement.x < 0 && !animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            spriteRenderer.flipX = true;

        }
    }

    private void move()
    {
        if (moveable && !animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            playerRB.MovePosition(playerRB.position + movement.normalized * movespeed * Time.fixedDeltaTime);
        }
        animator.SetFloat("Speed", (movement.normalized * movespeed * Time.fixedDeltaTime).magnitude * Convert.ToInt32(moveable));
    }
}