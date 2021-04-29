using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbility : MonoBehaviour
{   
    // Damage related
    private float damage;
    public float dmgMultiplier = 0.5f;
    public int numberOfHits = 3;

    // Cooldown related
    public float delay;
    private float timeSinceJump = 0f;
    private float cooldown = 0;

    // Range/Targeting related
    public float range = 5;
    public float chainRange = 4;
    Vector2 mousePoint;
    LayerMask enemyLayer;

    private BoxCollider2D playerCollider;
    private PlayerMovement playerMovement;
    private PlayerActions playerAction;

    private bool attacking = false;
    private int hitsMade = 0;
    private Vector3 oldPos;

    public GameObject slash;

    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        oldPos = transform.position;
        playerCollider = GetComponent<BoxCollider2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAction = GetComponent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !attacking && !playerAction.isActive)
        {
            
            mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 distVector = mousePoint - getPlayerPos();
            
            if (distVector.magnitude < range) 
            {
                playerAction.isActive = true;
                oldPos = transform.position;
                attacking = true;
                hitsMade = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (attacking) {
            timeSinceJump += Time.fixedDeltaTime;

            // attack if mouseclick
            if (hitsMade == 0)
            {
                playerCollider.enabled = false;
                playerMovement.moveable = false;
                Attack();
            }
            // quit if max nr of hits reached
            else if (hitsMade == numberOfHits && timeSinceJump > delay)
            {
                attacking = false;
                playerCollider.enabled = true;
                playerMovement.moveable = true;
                transform.position = oldPos;
                playerAction.isActive = false;

            }
            // Keep attacking if in the middle of an attack
            else if (timeSinceJump > delay) {
                Attack();
            }
        }
    }

    private void Attack()
    {
        Collider2D[] enemies;
        Vector2 startPos;

        // Get the position for where to check chain.
        if (hitsMade == 0)
        {
            startPos = mousePoint;
        } 
        else 
        {
            startPos = getPlayerPos();
        }
        
        enemies = Physics2D.OverlapCircleAll(startPos, chainRange, enemyLayer);
        
        // Teleport to closest enemy and damage it
        if (enemies.Length > 0)
        {
            // Find enemy and place player
            GameObject closest = findClosest(enemies);
            Vector3 newPos = closest.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
            transform.position = newPos;

            // Damage enemy
            Quaternion slashRotation = closest.transform.rotation * Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 180)));
            Instantiate(slash, closest.transform.position, slashRotation);
            closest.GetComponent<Enemy>().takeDamage(damage);
            hitsMade++;
        }
        else
        {
            // attack failed
            hitsMade = numberOfHits;
        }
        
        timeSinceJump = 0;
    }

    // Currently actually doesnt find the closest in order to solve a bug, can return to this later
    // Solution, for randomness: if we find the last enemy, place it first in the list and then pick
    // a random outside of that index.
    GameObject findClosest(Collider2D[] enemies)
    {
        if (enemies.Length == 1)
        {
            return enemies[0].gameObject;
        } 
        else 
        {
            return enemies[Random.Range(0, enemies.Length - 1)].gameObject;
        }
    }

    public void setDamage(int damage){
        this.damage = damage * dmgMultiplier;
    }

    private Vector2 getPlayerPos(){
        return new Vector2(transform.position.x, transform.position.y);
    }
}
