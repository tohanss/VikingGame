using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityAbility : MonoBehaviour
{
    // Damage related
    private float damage;
    private float dmgMultiplier = 0.5f;
    private Collider2D hitEnemy;

    // Cooldown related
    public int maxCharges;
    private int chargesLeft;
    public float dashCooldown;

    // Dash related
    public float dashDistance;
    public float dashForce;
    private Vector3 dashStartPos;
    private Vector2 facingDirection;
    private bool isDashing = false;
    private bool hitCollidable = false;

    // Misc related
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer spriteRenderer;
    private PlayerActions playerAction;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAction = GetComponent<PlayerActions>();
        chargesLeft = maxCharges;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && !playerAction.isActive)
        {
            if (chargesLeft > 0) 
            {
                chargesLeft--; //use one charge per dash
                StartCoroutine(replenishCharge()); //replenish a charge after a cooldown
                playerAction.isActive = true;
                playerMovement.moveable = false;
                isDashing = true;
                dashStartPos = transform.position;
                facingDirection = playerMovement.movement.normalized;
                //if you dash without pressing WASD, then dash in the direction of the sprite
                if (facingDirection == Vector2.zero)
                {
                    if (spriteRenderer.flipX == false)
                    {
                        facingDirection = Vector2.right;
                    }
                    else 
                    {
                        facingDirection = Vector2.left;
                    }
                }
            }
        }

    }
    void FixedUpdate()
    {
        if (isDashing) 
        {
            dash();
        }
    }

    private void dash() 
    {
        //moves the player, using rigidbody velocity. If not using velocity player will get stuck in collidables
        playerRigidBody.velocity = facingDirection * dashForce; 
        
        //Dash ends when player hits a collidable(not enemy) or has dashed the max distance
        if (Vector2.Distance(transform.position, dashStartPos) > dashDistance || hitCollidable)
        {
            playerRigidBody.velocity = Vector2.zero;
            isDashing = false;
            playerMovement.moveable = true;
            playerAction.isActive = false;
            hitCollidable = false;
            hitEnemy = null;
        }
    }

    public void setDamage(int damage)
    {
        this.damage = damage * dmgMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !(hitEnemy == other) && isDashing)
        {
            hitEnemy = other;
            other.GetComponent<Enemy>().takeDamage(damage);
        }
        else if (other.gameObject.CompareTag("Collidables"))
        {
            hitCollidable = true;
        }
    }
    //Using coroutine, works great. replenishes a dash charge after cooldown has passed
    IEnumerator replenishCharge() 
    {
        yield return new WaitForSeconds(dashCooldown);
        Debug.Log("1 Dash Charge replenished");
        chargesLeft++;
    }
}
