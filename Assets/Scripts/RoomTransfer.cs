using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransfer : MonoBehaviour
{
    private GameObject toolTip;

    public Vector2 cameraChangeMax;
    public Vector2 cameraChangeMin;
    public Vector3 playerChange;
    private CameraController cam;
    // Start is called before the first frame update
    void Start()
    {
        toolTip = gameObject.transform.GetChild(0).gameObject;
        cam = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            if (Input.GetKey(KeyCode.E))
            {
                // Updates camera boundries to the new map. The added values are for adjusting the camera just slightly to fit all tiles in the camera aspect.
                float height = 2f * Camera.main.orthographicSize;
                float width = height * (Screen.width / Screen.height);
                cam.minX = cameraChangeMin.x + (width / 2) + 0.6f;
                cam.minY = cameraChangeMin.y + (height / 2);
                cam.maxX = cameraChangeMax.x - (width / 2) + 0.4f;
                cam.maxY = cameraChangeMax.y - (height / 2) + 1;
                other.transform.position = playerChange;
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
}
