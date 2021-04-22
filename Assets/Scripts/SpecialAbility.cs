using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbility : MonoBehaviour
{

    private float cooldown = 0;
    private float damage = 4;
    public float dmgMultiplier = 0.5f;
    public float range = 7;
    public float chainRange = 4;
    public int numberOfHits = 3;
    public int delay = 12;

    private PlayerActions playerAction;
    private BoxCollider2D playerCollider;
    private PlayerMovement playerMovement;

    private GameObject[] enemies;
    private int hitsMade = 0;
    private Vector3 oldPos;
    private float timeCount = 0f;
    private GameObject lastTarget;
    private int firstAttack = 0; // -1 -> failed attack; 0 -> no attack; -> 1 attacking

    // Start is called before the first frame update
    void Start()
    {
        oldPos = transform.position;
        playerAction = GetComponent<PlayerActions>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerMovement = GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            firstAttack = 1;
        // update damage
        damage = playerAction.playerDamage * dmgMultiplier;
    }

    private void FixedUpdate()
    {
        timeCount++;
        // quit if max nr of hits reached
        if (hitsMade > numberOfHits - 1 && timeCount % delay == 0 && firstAttack != -1)
        {
            playerCollider.enabled = true;
            playerMovement.moveable = true;
            hitsMade = 0;
            transform.position = oldPos;
            lastTarget = null;
        }
        // attack if mouseclick
        else if (firstAttack == 1)
        {
            playerCollider.enabled = false;
            playerMovement.moveable = false;
            Attack();
        }
        // keep attacking if in the middle of an attack
        else if (hitsMade > 0 && timeCount % delay == 0 && firstAttack != -1)
            Attack();
    }

    void Attack()
    {
        // do the attack
        // save position if this is the first hit
        if (firstAttack == 1)
        {
            oldPos = transform.position;
        }
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length != 0)
        {
            // find closest enemy
            GameObject closest = findClosest(enemies);
            // teleport to closest enemy and damage it
            if (((transform.position - closest.transform.position).magnitude <= range && firstAttack == 1)
                    || (transform.position - closest.transform.position).magnitude <= chainRange)
            {
                // HIT TARGET
                lastTarget = closest;
                Vector3 newPos = closest.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
                transform.position = newPos;
                closest.GetComponent<Enemy>().takeDamage(damage);
                hitsMade++;
                firstAttack = 0;
            }
            // reset if no one is within range
            else
            {
                // attack failed
                if (firstAttack == 1) firstAttack = -1;
                hitsMade = 1000;
                timeCount = 0;
            }
        }
    }

    GameObject findClosest(GameObject[] enemies)
    {
        GameObject closest = enemies[0];
        float minDist = 10000;
        foreach (GameObject enemy in enemies)
        {
            float dist = (transform.position - enemy.transform.position).magnitude;
            if (dist < minDist)
            {
                closest = enemy;
                minDist = dist;
            }
        }
        if (closest == lastTarget)
        {
            int i = 0;
            GameObject newTarget = enemies[Random.Range(0, enemies.Length - 1)];
            while (((transform.position - newTarget.transform.position).magnitude > chainRange || newTarget == lastTarget) && i < 20)
            {
                i++;
                newTarget = enemies[Random.Range(0, enemies.Length - 1)];
            }
            if (i < 20)
                return newTarget;
        }
        return closest;
    }

}
