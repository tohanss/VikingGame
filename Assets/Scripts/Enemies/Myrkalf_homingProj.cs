using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myrkalf_homingProj : MonoBehaviour
{
    //Projectile attack stats and logic
    [HideInInspector]
    public float projectileDamage;
    private Collider2D hitPlayer;
    private float lifeTime = 3.0f;
    private GameObject playerCharacter;
    private Transform target;
    private float homingProjSpeed;

    // Homing-projectile spawn related
    private Vector3 targetDirection;
    private Vector3 startDirection;
    [HideInInspector]
    public float startingYOffset;
    [HideInInspector]
    public float startingXOffset;
    private float t = 0;

    // Start is called before the first frame update
    private void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        target = playerCharacter.GetComponent<Transform>();
        homingProjSpeed = 4f;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        spawnHomingProj();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !(hitPlayer == other))
        {
            hitPlayer = other;

            if (!other.GetComponent<PlayerActions>().isInvulnerable) //only do damage if player is not invulnerable
            {
                other.GetComponent<PlayerActions>().playerTakeDamage(projectileDamage);
                Destroy(gameObject);
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

    private void spawnHomingProj()
    {
        startDirection = (target.position - transform.position) + new Vector3(startingXOffset, startingYOffset, 0);
        targetDirection = (target.position - transform.position) + new Vector3(0, 0.5f, 0);

        Vector3 interpolated = (1 - t) * startDirection + t * targetDirection;
        Vector3 interpolated2 = (1 - t) * (startDirection*-1) + t * (targetDirection*-1);
        if (t < 1)
        {
            t += 0.005f;
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = interpolated.normalized * homingProjSpeed;
        float angle = Mathf.Atan2(interpolated2.y, interpolated2.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
