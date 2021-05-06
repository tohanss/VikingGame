using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public new string name;
    public string description;
    //public Sprite sprite;

    public int upgradeID;
    public RuntimeAnimatorController animator;
}
