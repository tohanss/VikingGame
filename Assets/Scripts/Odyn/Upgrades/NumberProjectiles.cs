using UnityEngine;

public class NumberProjectiles : MonoBehaviour
{
    private GameObject toolTip;

    private void Start() {
        toolTip = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                collidedWith.GetComponent<PlayerClass>().increaseProjectiles(1);
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
