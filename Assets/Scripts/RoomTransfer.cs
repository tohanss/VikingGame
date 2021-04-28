using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransfer : MonoBehaviour
{
    public Vector2 cameraChangeMax;
    public Vector2 cameraChangeMin;
    public Vector3 playerChange;
    private CameraController cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Camera.main.transform.position);
        
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            float height = 2f * Camera.main.orthographicSize;
            float width = height * (Screen.width / Screen.height);
            cam.minX = cameraChangeMin.x + (width / 2) + 1;
            cam.minY = cameraChangeMin.y + (height / 2);
            cam.maxX = cameraChangeMax.x - (width / 2);
            cam.maxY = cameraChangeMax.y - (height / 2) + 1;
            other.transform.position = playerChange;
        }
    }
}
