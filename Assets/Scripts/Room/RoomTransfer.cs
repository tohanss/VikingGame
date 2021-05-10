using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransfer : MonoBehaviour
{
    private GameObject toolTip;
    private Transform nextRoomPortalPos;

    public GameObject currentRoom;
    public GameObject nextRoom;
    public GameObject vcamFade;
    // Start is called before the first frame update
    void Start()
    {
        toolTip = gameObject.transform.GetChild(0).gameObject;
        nextRoomPortalPos = nextRoom.gameObject.transform.GetChild(1).gameObject.transform;
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            if (Input.GetKey(KeyCode.E))
            {
                vcamFade.SetActive(true);
                Invoke("enableNextRoomCamera", 1.1f);
                other.transform.position = nextRoomPortalPos.position;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            toolTip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            toolTip.SetActive(false);
        }
    }

    private void enableNextRoomCamera()
    {
        currentRoom.SetActive(false);
        vcamFade.SetActive(false);
        nextRoom.SetActive(true);
    }
}
