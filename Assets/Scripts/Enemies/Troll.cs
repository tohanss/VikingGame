using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Enemy
{
    //Troll specific stats
    private float slamDamage;
    // Misc


    protected override void Start()
    {
        base.Start();
        attackCooldown = 3.0f;
    }
    protected override void attack()
    {
        base.attack();
    }

}
