using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UtilityAbility : MonoBehaviour
{
    // Damage related
    private float damage;
    public float dmgMultiplier;
    private Collider2D hitEnemy;

    // Cooldown related
    public int maxCharges;
    [HideInInspector]
    public int chargesLeft;
    public float dashCooldown;
    private Image icon;
    private TMP_Text chargesText;
    private Image flash;

    // Dash related
    public float dashDistance;
    private float dashSpeed = 30f; //Should never be set to 0
    private Vector3 dashStartPos;
    private Vector2 facingDirection;
    [HideInInspector]
    public bool isDashing = false;
    [HideInInspector]
    public bool hitCollidable = false;

    // Misc
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer spriteRenderer;
    private PlayerActions playerAction;
    private Animator animator;
    public GameObject trail;
    private GameObject activeTrail;

    // Upgrade related
    public bool placeDot = false;
    public GameObject dotPrefab;

    public bool lifeLeech = false;

    public bool attackToSides = false;
    public GameObject projectilePrefab;
    private float movementSinceAttack = 0f;

    private AudioSource audioSource;
    public AudioClip dashSound;
    private AudioClip oldSound;
    private float oldVolume;
    private float oldPitch;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAction = GetComponent<PlayerActions>();
        animator = GetComponent<Animator>();
        chargesLeft = maxCharges;

        icon = playerAction.canvas.transform.GetChild(3).GetChild(2).GetChild(1).GetComponent<Image>();
        chargesText = playerAction.canvas.transform.GetChild(3).GetChild(2).GetChild(4).GetComponent<TMP_Text>();
        chargesText.text = maxCharges.ToString();
        flash = playerAction.canvas.transform.GetChild(3).GetChild(2).GetChild(2).GetComponent<Image>();
        flash.color = new Color(1, 1, 1, 0);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && !playerAction.isActive)
        {
            if (chargesLeft > 0)
            {   
                audioSource.PlayOneShot(dashSound, 12f);

                chargesLeft--; //use one charge per dash
                chargesText.text = chargesLeft.ToString();
                if(chargesLeft < 1)
                {
                    StartCoroutine(coolDown());
                }

                StartCoroutine(replenishCharge()); //replenish a charge after a cooldown
                playerAction.isInvulnerable = true;
                playerAction.isActive = true;
                playerAction.moveable = false;
                movementSinceAttack = 0f;
                isDashing = true;
                dashStartPos = transform.position;
                facingDirection = playerAction.movement.normalized;
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
                //Play the appropriate animation
                if (0 == Vector3.Dot( facingDirection, Vector3.right))
                {
                        if(facingDirection == Vector2.up)
                        {
                            animator.SetTrigger("UtilityUp");
                        }
                        else
                        {
                            animator.SetTrigger("UtilityDown");
                        }
                }
                else
                {
                    animator.SetTrigger("UtilityRight");
                }
                // give the player a trail
                activeTrail = Instantiate(trail, gameObject.transform);
                activeTrail.transform.localPosition += Vector3.up * 0.6f;
                if(spriteRenderer.flipX == false)
                {
                    activeTrail.transform.localPosition += Vector3.right * 0.09f;
                } else activeTrail.transform.localPosition += Vector3.left * 0.09f;
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
        playerRigidBody.velocity = facingDirection * dashSpeed;
        // Adjust the animation length accordingly
        animator.SetFloat("DashSpeed", 0.05f / (playerRigidBody.velocity.magnitude * Time.deltaTime * dashDistance * Time.deltaTime));

        if (attackToSides && movementSinceAttack > 1f)
        {
            Vector2 left = Quaternion.AngleAxis(90, Vector3.forward) * facingDirection;
            Vector2 right = Quaternion.AngleAxis(-90, Vector3.forward) * facingDirection;

            Rigidbody2D rbLeft = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<Rigidbody2D>();        
            Rigidbody2D rbRight = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
            
            rbLeft.velocity = left * 10f;
            rbRight.velocity = right * 10f;

            movementSinceAttack = 0f;
        }
        else
        {
            movementSinceAttack = Vector2.Distance(transform.position, dashStartPos);
        }
        
        //Dash ends when player hits a collidable(not enemy) or has dashed the max distance
        if (Vector2.Distance(transform.position, dashStartPos) > dashDistance)
        {
            resetDash();
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

            if (lifeLeech)
            {
                gameObject.GetComponent<PlayerActions>().playerRestoreHealth(Mathf.Ceil(damage * 0.5f));
            }

            if (placeDot)
            {
                Instantiate(dotPrefab, other.transform);
            }
        }
      
    }

    //Using coroutine, works great. replenishes a dash charge after cooldown has passed
    IEnumerator replenishCharge() 
    {
        yield return new WaitForSeconds(dashCooldown);
        chargesLeft++;
        chargesText.text = chargesLeft.ToString();
        flash.color = Color.white;
        for (int i = 0; i < 6; i++)
        {
            flash.color = new Color(1, 1, 1, (1.0f - i / 5.0f));
            yield return new WaitForSeconds(0.05f);
        }
        flash.color = new Color(1, 1, 1, 0);
    }

    public void resetDash()
    {
        if (hitCollidable)
        {
            animator.Play("player-idle");
        }
        playerRigidBody.velocity = Vector2.zero;
        isDashing = false;
        playerAction.moveable = true;
        playerAction.isInvulnerable = false;
        playerAction.isActive = false;
        hitCollidable = false;
        hitEnemy = null;
        Destroy(activeTrail, 0.1f);
    }

    IEnumerator coolDown()
    {
        icon.color = new Color(1, 1, 1, 0.3f);
        while (chargesLeft < 1)
        {
            yield return new WaitForFixedUpdate();
        }
        icon.color = Color.white;
    }

    public IEnumerator increaseCharge()
    {
        chargesLeft++;
        chargesText.text = chargesLeft.ToString();
        flash.color = Color.white;
        for (int i = 0; i < 6; i++)
        {
            flash.color = new Color(1, 1, 1, (1.0f - i / 5.0f));
            yield return new WaitForSeconds(0.05f);
        }
        flash.color = new Color(1, 1, 1, 0);
    }
}
