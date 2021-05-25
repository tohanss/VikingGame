using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.LWRP;

public class TrailerUpgrade : MonoBehaviour
{
    public Upgrade thisUpgrade;
    private UpgradeDropper upgradeDropper;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                PlayerClass player = collidedWith.GetComponent<PlayerClass>();

                switch (thisUpgrade.upgradeID)
                {
                    case 0:
                        player.setPierce(true);
                        break;
                    case 1:
                        player.increaseProjectiles(1);
                        break;
                    case 2:
                        player.increaseSpecialHits(1);
                        break;
                    case 3:
                        player.increaseUtilityCharges(1);
                        break;
                    case 4:
                        player.setDoubleDamageInSpearRange(true);
                        break;
                    case 5:
                        player.setCrowDotEffect(true);
                        break;
                    case 6:
                        player.increaseAttackSpeed(1.2f);
                        break;
                    case 7:
                        player.setSpecialDotEffect(true);
                        break;
                    case 8:
                        player.increaseSpecialAoE();
                        break;
                    case 9:
                        player.setUtilityDot();
                        break;
                    case 10:
                        player.setUtilityLeech();
                        break;
                    case 11:
                        player.increaseUtilityRange(1f);
                        break;
                    case 12:
                        player.setUtilityAttackToSides();
                        break;
                    case 13:
                        player.decreaseCooldownShadowstep();
                        break;
                    default:
                        break;
                }
                Destroy(gameObject);
            }
        }
    }

}
