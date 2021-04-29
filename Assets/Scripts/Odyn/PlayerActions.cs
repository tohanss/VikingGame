using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    // Connected objects
    private PlayerClass playerClass;

    // Player stats
    private int level = 1;

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
}