using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollSlamAttack : MonoBehaviour
{

    //Slam attack stats and logic
    private float slamDamage;
    private Collider2D hitPlayer;
    private ParticleSystem particleSystem;
    private float lifeTime;


    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        var main = particleSystem.main;
        lifeTime = main.startLifetime.constant;
        Destroy(gameObject, lifeTime);
    }

    public void setDamage(float value)
    {
        slamDamage = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !(hitPlayer == other))
        {
            hitPlayer = other;
            if (!other.GetComponent<PlayerActions>().isInvulnerable) //only do damage if player is not invulnerable
            {
                other.GetComponent<PlayerActions>().playerTakeDamage(slamDamage);
            }
        }
    }
}
