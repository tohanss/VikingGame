using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityAbility : MonoBehaviour
{
    // Damage related
    private float damage;

    // Cooldown related
    public int dashCharges = 1;
    private float dashCooldown = 1;
    private float lastTime = 0;

    // Dash related
    public float dashDistance;
    public float dashForce;
    private Vector3 dashStartPos;
    private Vector2 facingDirection;
    private bool isDashing = false;

    // Misc related
    private BoxCollider2D playerCollider;
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            if (Time.time - lastTime > dashCooldown) 
            {
                playerMovement.moveable = false;
                isDashing = true;
                dashStartPos = transform.position;
                facingDirection = playerMovement.movement.normalized;
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
        playerRigidBody.velocity = facingDirection * dashForce;
        
        if (Vector2.Distance(transform.position, dashStartPos) > dashDistance)
        {
            lastTime = Time.time;
            playerRigidBody.velocity = Vector2.zero;
            isDashing = false;
            playerMovement.moveable = true;
        }
    }

    

    public void setDamage(int damage)
    {
        this.damage = damage;
    }
}
