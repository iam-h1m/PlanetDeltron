using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private PlayerController playerController;

    public float trailDistance;
    public float heightOffset;
    // Smooth transition for camera at turns
    private float cameraDelay = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        // Get characterController script
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Check if game is over
        if (!playerController.isGameOver)
        {
            // Set camera behind player
            Vector3 followPos = target.position - target.forward * trailDistance;

            followPos.y += heightOffset;
            transform.position += (followPos - transform.position) * cameraDelay;

            transform.LookAt(target.transform);
        }
    }
}
