using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDropper : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public GeneralUpgrade upgradePrefab;

    public void dropUpgrade()
    {
        Instantiate(upgradePrefab, gameObject.transform);
    }

    public void remove(Upgrade upgrade)
    {
        upgrades.Remove(upgrade);
    }
}
