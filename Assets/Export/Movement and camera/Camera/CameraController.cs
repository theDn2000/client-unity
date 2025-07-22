using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 8f;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not set for CameraController.");
            return;
        }

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + offset.y, target.position.z) + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
