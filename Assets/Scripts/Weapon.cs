using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            ThrowSpear();
        }
    }  

    void ThrowSpear(){
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
