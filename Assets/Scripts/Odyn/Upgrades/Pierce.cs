using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pierce : MonoBehaviour
{
    private GameObject toolTip;

    private void Start() {
        toolTip = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (Input.GetKey(KeyCode.E))
        {
            GameObject collidedWith = other.gameObject;
            if (collidedWith.tag == "Player")
            {
                collidedWith.GetComponent<PlayerClass>().setPierce(true);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.tag == "Player")
        {
            toolTip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.tag == "Player")
        {
            toolTip.SetActive(false);
        }
    }
}
