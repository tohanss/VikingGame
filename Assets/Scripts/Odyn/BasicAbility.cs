using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAbility : MonoBehaviour
{
    // Basic attack
    public Transform firePoint;
    public GameObject projectilePrefab;
    private SpearProjectile projectileScript;

    // Upgrade related
    public bool basicPierce;
    public int numberProjectiles = 1;

    // Attack stats
    private float scatterMaxAngle = 30;
    private float projectileSpeed = 10f;

    void Start()
    {
        projectileScript = projectilePrefab.GetComponent<SpearProjectile>();

        // Apply pierce upgrade to prefab
        setPierce(basicPierce);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            Attack();
        }
    }

    // Attack Related
    private void Attack()
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

    public void setDamage(int damage){
        projectileScript.damage = damage;
    }

    public void setPierce(bool value){
        projectileScript.pierce = value;
    }
}
