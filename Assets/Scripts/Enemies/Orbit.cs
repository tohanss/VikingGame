using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject rotateAroundObject;
    [HideInInspector]
    public float orbDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void orbitAround()
    {

    }

    public void setDamage(float value)
    {
        orbDamage = value;
    }
}
