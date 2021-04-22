using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    // Basic attack
    public Transform firePoint;
    public GameObject projectilePrefab;
    private SpearProjectile projectileScript;

    public float scatterMaxAngle = 30;

    // Player stats
    public int playerDamage = 10;
    private int level = 1;
    private float projectileSpeed = 10f;

    // Experience and level related
    private int currentExp = 0;
    private int requiredExp = 10;
    public Text favorText;
    public Text levelText;
    
    // Upgrade related
    public bool basicPierce;
    public int numberProjectiles = 1;

    // Unity functions
    private void Start() 
    {
        favorText.text = "Favour: " + currentExp.ToString() + " / " + requiredExp.ToString();
        levelText.text = "Level: " + level.ToString();

        projectileScript = projectilePrefab.GetComponent<SpearProjectile>();

        // Set stats on projectile prefab
        projectileScript.damage = playerDamage;

        // Apply pierce upgrade to prefab
        projectileScript.pierce = basicPierce;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            BasicAttack();
        }
    }  

    // Attack Related
    private void BasicAttack()
    {   
        // Set start rotation
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position);
        direction = Quaternion.AngleAxis(scatterMaxAngle, Vector3.forward) * direction;

        // Set rotation appropriate for number of projectiles
        float rotationStep = (scatterMaxAngle*2)/(numberProjectiles+1);

        for (int i = 0; i < numberProjectiles; i++)
        {   
            Rigidbody2D rb = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<Rigidbody2D>();        
            
            // modify direction to spread out projectiles
            direction = Quaternion.AngleAxis(-rotationStep, Vector3.forward) * direction;

            rb.velocity = direction.normalized * projectileSpeed;
        }
        
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
        playerDamage += 2;
        projectileScript.damage = playerDamage;

        //Modify Exp values
        currentExp -= requiredExp;
        requiredExp = 10 + (level * 5);

        // Change level text
        levelText.text = "Level: " + level.ToString();
    }
}