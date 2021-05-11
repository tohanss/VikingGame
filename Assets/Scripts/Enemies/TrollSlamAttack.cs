using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollSlamAttack : MonoBehaviour
{

    //Slam attack stats and logic
    [HideInInspector]
    public float slamDamage;
    private Collider2D hitPlayer;
    private ParticleSystem ps;
    private float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(colliderTimer());
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
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
    //Handles turning off collider, otherwise player can take damage as long as the particle effect is still alive
    IEnumerator colliderTimer() 
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
