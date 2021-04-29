using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        thisUpgrade = upgrades[ Random.Range(0, upgrades.Length)];
        toolTip.transform.GetChild(1).GetComponent<TextMeshPro>().SetText( thisUpgrade.description, true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                switch (thisUpgrade.upgradeID)
                {
                    case 0:
                        collidedWith.GetComponent<PlayerClass>().setPierce(true);
                        break;
                    default:
                        collidedWith.GetComponent<PlayerClass>().increaseProjectiles(1);
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
}
