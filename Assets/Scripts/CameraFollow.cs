using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    Vector3 veclocity = Vector3.zero;

    public float smoothTime = .15f;
    
    public float minX, minY, maxX, maxY;

    private void FixedUpdate()
    {
      
        Vector3 clampedPos = new Vector3(Mathf.Clamp(target.position.x, minX, maxX), Mathf.Clamp(target.position.y, minY, maxY), -1);

        transform.position = Vector3.SmoothDamp(transform.position, clampedPos, ref veclocity, smoothTime);
        
    }
}
