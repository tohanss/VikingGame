using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.LWRP;

public class GeneralUpgrade : MonoBehaviour
{
    private GameObject toolTip;
    [HideInInspector]
    public Upgrade thisUpgrade;
    private UpgradeDropper upgradeDropper;

    public int dropIndex;
    
    private void Start()
    {
        upgradeDropper = GameObject.Find("UpgradeDropper(Clone)").GetComponent<UpgradeDropper>(); 
        
        toolTip = gameObject.transform.GetChild(0).gameObject;
        toolTip.transform.GetChild(1).GetComponent<TextMeshPro>().SetText(thisUpgrade.description, true);
        toolTip.transform.GetChild(2).GetComponent<TextMeshPro>().SetText(thisUpgrade.abilityName, true);

        GetComponent<Animator>().runtimeAnimatorController = thisUpgrade.animator;
        StartCoroutine(decreaseLight());
    }

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
                        upgradeDropper.remove(thisUpgrade);
                        break;
                    case 1:
                        player.increaseProjectiles(1);
                        if (player.basicProjectileUpgrades > 4)
                            upgradeDropper.remove(thisUpgrade);
                        break;
                    case 2:
                        player.increaseSpecialHits(1);
                        break;
                    case 3:
                        player.increaseUtilityCharges(1);
                        break;
                    case 4:
                        player.setDoubleDamageInSpearRange(true);
                        upgradeDropper.remove(thisUpgrade);
                        break;
                    case 5:
                        player.setCrowDotEffect(true);
                        upgradeDropper.remove(thisUpgrade);
                        break;
                    case 6:
                        player.increaseAttackSpeed(1.2f);
                        break;
                    case 7:
                        player.setSpecialDotEffect(true);
                        upgradeDropper.remove(thisUpgrade);
                        break;
                    case 8:
                        player.increaseSpecialAoE();
                        break;
                    case 9:
                        player.setUtilityDot();
                        upgradeDropper.remove(thisUpgrade);
                        break;
                    case 10:
                        player.setUtilityLeech();
                        upgradeDropper.remove(thisUpgrade);
                        break;
                    case 11:
                        player.increaseUtilityRange(1f);
                        break;
                    case 12:
                        player.setUtilityAttackToSides();
                        upgradeDropper.remove(thisUpgrade);
                        break;
                    case 13:
                        player.decreaseCooldownShadowstep();
                        if (player.shadowStepUpgrades > 2)
                            upgradeDropper.remove(thisUpgrade);
                        break;
                    default:
                        break;
                }
                upgradeDropper.destroyDrops(dropIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.tag == "Player")
        {
            toolTip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.tag == "Player")
        {
            toolTip.SetActive(false);
        }
    }

    IEnumerator decreaseLight()
    {
        UnityEngine.Experimental.Rendering.Universal.Light2D light = transform.GetChild(1).GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        yield return new WaitForSeconds(0.2f);
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            light.intensity -= 1;
            if (light.intensity < 2)
            {
                break;
            }
        }
    }
}
