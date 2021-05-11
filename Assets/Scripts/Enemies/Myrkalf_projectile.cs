using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myrkalf_projectile : MonoBehaviour
{   
    //Projectile attack stats and logic
    [HideInInspector]
    public float projectileDamage;
    private Collider2D hitPlayer;
    private float lifeTime = 2.0f;

    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !(hitPlayer == other))
        {
            hitPlayer = other;

            if (!other.GetComponent<PlayerActions>().isInvulnerable) //only do damage if player is not invulnerable
            {
                other.GetComponent<PlayerActions>().playerTakeDamage(projectileDamage);
            }
        }
        else if (other.gameObject.CompareTag("HighCollidables"))
        {
            Destroy(gameObject);
        }
    }
    public void setDamage(float value)
    {
        projectileDamage = value;
    }
}
