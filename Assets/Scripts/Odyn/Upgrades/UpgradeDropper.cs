using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDropper : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public GeneralUpgrade upgradePrefab;
    private List<GeneralUpgrade[]> currentDrops;
    private int numDroppedUpgrades;

    private AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip dropSound;

    private void Start() {
        numDroppedUpgrades = 0;
        currentDrops = new List<GeneralUpgrade[]>();

        audioSource = GetComponent<AudioSource>();
    }

    public void dropUpgrade(Vector2 offset)
    {   
        audioSource.PlayOneShot(dropSound, 0.70f);

        numDroppedUpgrades += 1;

        currentDrops.Add(new GeneralUpgrade[3]);

        Vector2 spawnPosition = GameObject.Find("Player").transform.position;

        currentDrops[numDroppedUpgrades-1][0] = Instantiate(upgradePrefab, spawnPosition + offset, Quaternion.identity);
        currentDrops[numDroppedUpgrades-1][1] = Instantiate(upgradePrefab, spawnPosition + offset + new Vector2(3f,0), Quaternion.identity);
        currentDrops[numDroppedUpgrades-1][2] = Instantiate(upgradePrefab, spawnPosition + offset + new Vector2(-3f,0), Quaternion.identity);

        // Generates the upgrades in a way that all three are unique
        HashSet<int> upgradeNumbers = new HashSet<int>();

        while (upgradeNumbers.Count < 3)
        {
            upgradeNumbers.Add(Random.Range(0, upgrades.Count));
        }

        int pos = 0;
        foreach (int number in upgradeNumbers)
        {
            currentDrops[numDroppedUpgrades-1][pos].thisUpgrade = upgrades[number];
            pos++;
        }

        // Tells all upgrades what order they were dropped in, so that we can remove the associated
        // upgrades when picking one up
        for (int i = 0; i < 3; i++)
        {
            currentDrops[numDroppedUpgrades-1][i].dropIndex = numDroppedUpgrades-1;
        }

    }

    public void destroyDrops(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            Destroy(currentDrops[index][i].gameObject);
        }
    }

    public void remove(Upgrade upgrade)
    {
        upgrades.Remove(upgrade);
    }

    public void playPickupSound(){
        // Had to be here because upgrades are destroyed before they can play the sound
        audioSource.PlayOneShot(pickupSound, 0.70f);
    }
}
