using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.LWRP;

public class GeneralUpgrade : MonoBehaviour
{
    private GameObject toolTip;
    public Upgrade[] upgrades;
    private Upgrade thisUpgrade;
    

    private void Start()
    {
        toolTip = gameObject.transform.GetChild(0).gameObject;
        // min is incluseive, max is exclusive
        // picks a random upgrade for this instance from the list upgrades
        int upgradeID = Random.Range(0, upgrades.Length);
        thisUpgrade = upgrades[upgradeID];
        toolTip.transform.GetChild(1).GetComponent<TextMeshPro>().SetText(thisUpgrade.description, true);
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
                        player.upgradeAoeEffect();
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
                    default:
                        break;
                }
                Destroy(gameObject);
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
            Debug.Log("inne");
            if (light.intensity < 2)
            {
                break;
            }
        }
    }
}
