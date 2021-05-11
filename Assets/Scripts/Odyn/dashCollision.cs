using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashCollision : MonoBehaviour
{
    private UtilityAbility dashScript;
    private void Start()
    {
        dashScript = gameObject.GetComponentInParent<UtilityAbility>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (dashScript.isDashing && (other.gameObject.CompareTag("Collidables") || other.gameObject.CompareTag("HighCollidables")))
        {
            dashScript.hitCollidable = true;
            dashScript.resetDash();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (dashScript.isDashing && (other.gameObject.CompareTag("Collidables") || other.gameObject.CompareTag("HighCollidables")))
        {
            dashScript.hitCollidable = true;
            dashScript.resetDash();

        }
    }
}
