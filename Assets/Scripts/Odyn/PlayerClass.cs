using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    private BasicAbility basicAbility;
    private SpecialAbility specialAbility;
    private UtilityAbility utilityAbility;

    // Upgrade dropper related
    public GameObject upgradeDropperPrefab;
    [HideInInspector]
    public UpgradeDropper upgradeDropper;

    // Stats
    private int playerDamage = 10;
    public float maxHealth;  //the class starting max health

    // Keep track of how many upgrades have been given
    public int basicProjectileUpgrades = 0;

    // Start is called before the first frame update
    void Start()
    {
        basicAbility = GetComponent<BasicAbility>();
        specialAbility = GetComponent<SpecialAbility>();
        utilityAbility = GetComponent<UtilityAbility>();

        upgradeDropper = Instantiate(upgradeDropperPrefab).GetComponent<UpgradeDropper>();

        // Initialises ability damages 
        increaseDamage(0);
    }

    public void increaseDamage(int amount){
        playerDamage += amount;
        basicAbility.setDamage(playerDamage);
        specialAbility.setDamage(playerDamage);
        utilityAbility.setDamage(playerDamage);
    }

    public void setPierce(bool value)
    {
        basicAbility.setPierce(value);
    }

    public void setDoubleDamageInSpearRange(bool value)
    {
        basicAbility.setDoubleDamageInSpearRange(value);
    }

    public void setCrowDotEffect(bool value)
    {
        basicAbility.setCrowDotEffect(value);
    }

    public void setSpecialDotEffect(bool value)
    {
        specialAbility.placeDot = true;
    }

    public void increaseSpecialAoE()
    {
        specialAbility.upgradeAoeEffect();
    }

    public void setUtilityDot()
    {
        utilityAbility.placeDot = true;
    }

    public void setUtilityLeech()
    {
        utilityAbility.lifeLeech = true;
    }

    public void setUtilityAttackToSides()
    {
        utilityAbility.attackToSides = true;
    }

    public void increaseProjectiles(int times)
    {
        basicAbility.numberProjectiles += times;
    } 

    public void increaseSpecialHits(int amount)
    {
        specialAbility.numberOfHits += amount;
        specialAbility.delay *= Mathf.Pow(0.9f, amount);
    }

    public void increaseUtilityCharges(int amoount)
    {
        utilityAbility.maxCharges += 1;
        utilityAbility.chargesLeft += 1;
    }

    public void increaseUtilityRange(float amount)
    {
        utilityAbility.dashDistance += amount;
    }

    public void increaseAttackSpeed(float multiplier)
    {
        basicAbility.increaseAttackSpeed(multiplier);
    }

    public int getDamage(){
        return playerDamage;
    }

    public void dropUpgrade(int level)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        Vector3 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        upgradeDropper.dropUpgrade();
    }
}
