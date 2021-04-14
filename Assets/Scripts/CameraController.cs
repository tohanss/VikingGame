using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothValue;
    public float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //LateUpdate is called after all Update functions have been called. This is useful to order script execution. 
    //For example a follow camera should always be implemented in LateUpdate because it tracks objects that might have moved inside Update.
    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        //move camera smoothly
        //param1:Current position
        //param2: Target position
        //param3: Value used to interpolate between a and b
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothValue*Time.deltaTime);


        //Bounding the Camera, limits the camera range based on param minX,maxX,minY,maxY
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
                                         Mathf.Clamp(transform.position.y, minY, maxY),
                                         transform.position.z);


    }
}
