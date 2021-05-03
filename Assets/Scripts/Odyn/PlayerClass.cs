using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    private BasicAbility basicAbility;
    private SpecialAbility specialAbility;
    private UtilityAbility utilityAbility;

    public GameObject upgrade;

    // Stats
    private int playerDamage = 10;
    public float maxHealth;  //the class starting max health

    // Start is called before the first frame update
    void Start()
    {
        basicAbility = GetComponent<BasicAbility>();
        specialAbility = GetComponent<SpecialAbility>();
        utilityAbility = GetComponent<UtilityAbility>();

        // Initialises ability damages 
        increaseDamage(0);
    }

    public void increaseDamage(int amount){
        playerDamage += amount;
        basicAbility.setDamage(playerDamage);
        specialAbility.setDamage(playerDamage);
        utilityAbility.setDamage(playerDamage);

    }

    public void increaseProjectiles(int times)
    {
        basicAbility.numberProjectiles += times;
    } 

    public void setPierce(bool value)
    {
        basicAbility.setPierce(value);
    }

    public void increaseSpecialHits(int amount)
    {
        specialAbility.numberOfHits += amount;
        specialAbility.delay *= 0.9f;
    }

    public void increaseUtilityCharges(int amoount)
    {
        utilityAbility.maxCharges += 1;
        utilityAbility.chargesLeft += 1;
    }

    public int getDamage(){
        return playerDamage;
    }

    public void dropUpgrade(int level)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        Vector3 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Instantiate(upgrade, transform.position + offset, transform.rotation);
    }
}
