using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    private BasicAbility basicAbility;
    private SpecialAbility specialAbility;

    // Stats
    private int playerDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        basicAbility = GetComponent<BasicAbility>();
        specialAbility = GetComponent<SpecialAbility>();

        // Initialises ability damages 
        increaseDamage(0);
    }

    public void increaseDamage(int amount){
        playerDamage += amount;
        basicAbility.setDamage(playerDamage);
        specialAbility.setDamage(playerDamage);
    }

    public void increaseProjectiles(int times)
    {
        basicAbility.numberProjectiles += times;
    } 

    public void setPierce(bool value)
    {
        basicAbility.setPierce(value);
    }

    public int getDamage(){
        return playerDamage;
    }


}
