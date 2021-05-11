using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDropper : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public GeneralUpgrade upgradePrefab;
    private GeneralUpgrade[] currentDrops;

    public void dropUpgrade(Vector2 offset)
    {   
        if (currentDrops != null)
        {
            destroyDrops();
        }

        currentDrops = new GeneralUpgrade[3];

        Vector2 spawnPosition = GameObject.Find("Player").transform.position;

        currentDrops[0] = Instantiate(upgradePrefab, spawnPosition + offset, Quaternion.identity);
        currentDrops[1] = Instantiate(upgradePrefab, spawnPosition + offset + new Vector2(3f,0), Quaternion.identity);
        currentDrops[2] = Instantiate(upgradePrefab, spawnPosition + offset + new Vector2(-3f,0), Quaternion.identity);
    }

    public void destroyDrops()
    {
        for (int i = 0; i < 3; i++)
        {
            Destroy(currentDrops[i].gameObject);
        }
        currentDrops = null;
    }

    public void remove(Upgrade upgrade)
    {
        upgrades.Remove(upgrade);
    }
}
