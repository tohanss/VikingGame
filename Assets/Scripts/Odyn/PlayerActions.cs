using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    // Connected objects
    private PlayerClass playerClass;

    // Player stats
    private int level = 1;
    private float currentHealth; 


    [HideInInspector]
    // Bool for if an ability is currently in use
    public bool isActive = false;

    // Experience and level related
    private int currentExp = 0;
    private int requiredExp = 10;
    public Text favorText;
    public Text levelText;

    // Unity functions
    private void Start() 
    {
        
        favorText.text = "Favour: " + currentExp.ToString() + " / " + requiredExp.ToString();
        levelText.text = "Level: " + level.ToString();

        playerClass = GetComponent<PlayerClass>();
        currentHealth = playerClass.maxHealth;
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
        Debug.Log("Damage Taken: " + damage);

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            die();
        }
    }

    //temporary death, restarts level if you died
    private void die()
    {
        Debug.Log("You Died");
        Debug.Log("Restarting Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart to current scene

    }
}