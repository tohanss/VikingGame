using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public string abilityName;
    public string description;
    //public Sprite sprite;

    public int upgradeID;
    public RuntimeAnimatorController animator;
}
