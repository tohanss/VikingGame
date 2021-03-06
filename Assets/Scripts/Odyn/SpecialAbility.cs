using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAbility : MonoBehaviour
{   
    // Damage related
    private float damage;
    public float dmgMultiplier = 0.5f;
    
    // Cooldown related
    public int numberOfHits;
    public float delay;
    private float timeSinceJump = 0f;
    private float cooldown = 5;
    bool onCooldown = false;

    // Range/Targeting related
    public float range = 5;
    public float chainRange = 4;
    Vector2 mousePoint;
    LayerMask enemyLayer;

    private BoxCollider2D playerCollider;
    private PlayerActions playerAction;

    private bool attacking = false;
    private int hitsMade = 0;
    private Vector3 oldPos;

    public GameObject slash;

    // Upgrade related
    public GameObject dotPrefab;
    public bool placeDot;

    public bool areaOfEffect;
    public float aoeRadius = 1f;

    //Misc
    private Animator animator;
    private Image icon;
    private Image flash;

    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        oldPos = transform.position;
        playerCollider = GetComponent<BoxCollider2D>();
        playerAction = GetComponent<PlayerActions>();
        animator = GetComponent<Animator>();
        icon = playerAction.canvas.transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<Image>();
        flash = playerAction.canvas.transform.GetChild(3).GetChild(1).GetChild(2).GetComponent<Image>();
        flash.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !attacking && !playerAction.isActive && !onCooldown)
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
                playerAction.moveable = false;
                Attack();
            }
            // quit if max nr of hits reached
            else if (hitsMade == numberOfHits && timeSinceJump > delay)
            {
                attacking = false;
                playerCollider.enabled = true;
                playerAction.moveable = true;
                transform.position = oldPos;
                playerAction.isActive = false;

            }
            // Keep attacking if in the middle of an attack
            else if (timeSinceJump > delay)
            {
                if(!onCooldown) StartCoroutine(coolDown());
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

            animator.SetTrigger("Special");

            // Place dot if upgraded
            if (placeDot)
            {
                Instantiate(dotPrefab, closest.transform);
            }

            // Deal AoE damage to nearby enemies if upgraded.
            if (areaOfEffect)
            {
                Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(closest.transform.position, aoeRadius, enemyLayer);

                foreach (var nearby in nearbyEnemies)
                {
                    if (nearby.gameObject.GetInstanceID() != closest.GetInstanceID())
                    {
                        nearby.GetComponent<Enemy>().takeDamage(damage * 0.5f);
                    }
                }
            }
            
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

    IEnumerator coolDown()
    {
        icon.fillAmount = 0;
        icon.color = new Color(1, 1, 1, 0.5f);
        float loops = cooldown * 10;
        onCooldown = true;
        for(int i = 0; i < loops; i++)
        {
            yield return new WaitForSeconds(0.1f);
            icon.fillAmount = icon.fillAmount + 1 / loops;
        }
        onCooldown = false;
        // flash to tell the player it's ready for use
        flash.color = Color.white;
        icon.color = Color.white;
        for (int j = 0; j < 6; j++)
        {
            flash.color = new Color(1, 1, 1, (1.0f - j / 5.0f));
            yield return new WaitForSeconds(0.05f);
        }
        flash.color = new Color(1, 1, 1, 0);
    }

    public void setDamage(int damage){
        this.damage = damage * dmgMultiplier;
    }
    
    public void upgradeAoeEffect()
    {
        aoeRadius += 0.5f;
        areaOfEffect = true;
    }
    // should not go lower than 1
    public void decreaseCooldown()
    {
        if(cooldown > 1)
        {
            cooldown--;
        }
    }

    private Vector2 getPlayerPos(){
        return new Vector2(transform.position.x, transform.position.y);
    }
}
