using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDropper : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public GeneralUpgrade upgradePrefab;

    public void dropUpgrade(Vector2 offset)
    {
        Vector2 spawnPosition = GameObject.Find("Player").transform.position;

        Instantiate(upgradePrefab, spawnPosition + offset, Quaternion.identity);
    }

    public void remove(Upgrade upgrade)
    {
        upgrades.Remove(upgrade);
    }
}
