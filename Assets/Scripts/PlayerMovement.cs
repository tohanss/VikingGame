using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float movespeed = 5.0f;
    public Rigidbody2D rb;
    Vector2 movement;
    public bool moveable = true;
    public Animator animator;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    private void Start() 
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void FixedUpdate()
    {
        if (moveable && !animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            rb.MovePosition(rb.position + movement.normalized * movespeed * Time.fixedDeltaTime);
        }
        animator.SetFloat("Speed", (movement.normalized * movespeed * Time.fixedDeltaTime).magnitude * Convert.ToInt32(moveable));
    }

    void Move() 
    {
        movement.x = Input.GetAxisRaw("Horizontal"); //when moving right movement.x = 1 and left movement.x = -1
        movement.y = Input.GetAxisRaw("Vertical");

        //Don't turn when in attack animation, even if you are eg. holding "D" (moving right)
        if (movement.x > 0 && !animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            spriteRenderer.flipX = false;

        }
        if (movement.x < 0 && !animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            spriteRenderer.flipX = true;

        }
    }
}
