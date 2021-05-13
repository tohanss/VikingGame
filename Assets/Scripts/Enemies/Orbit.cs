using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject rotateAroundObject;
    private float orbitSpeed;
    private Collider2D hitPlayer;
    private float lastTimeHit = 0;
    [HideInInspector]
    public float orbDamage;

    // Start is called before the first frame update
    void Start()
    {
        orbitSpeed = 180.0f;
    }

    // Update is called once per frame
    void Update()
    {
        orbitAround();
    }

    //Spin the object around the target at orbitSpeed degrees/second.
    private void orbitAround()
    {
        transform.RotateAround(rotateAroundObject.transform.position + new Vector3(0, 0.5f, 0), Vector3.forward, orbitSpeed * Time.deltaTime);
    }

    public void setDamage(float value)
    {
        orbDamage = value;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastTimeHit >= 1)
            {
                if (!other.GetComponent<PlayerActions>().isInvulnerable) //only do damage if player is not invulnerable
                {
                    lastTimeHit = Time.time;
                    other.GetComponent<PlayerActions>().playerTakeDamage(orbDamage);
                }
            }

            
        }
    }
}
