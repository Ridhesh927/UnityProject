using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 clickPosition = Pointer.current.position.ReadValue();

            // 1. Check AR Foundation Plane Raycast
            if (raycastManager != null && raycastManager.Raycast(clickPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                Instantiate(objectToSpawn, hitPose.position, hitPose.rotation);
            }
            // 2. Direct Fallback: Create dynamic raycast against virtual ground plane (Y = 0)
            else if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(clickPosition);
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

                if (groundPlane.Raycast(ray, out float enterDistance))
                {
                    Vector3 spawnPoint = ray.GetPoint(enterDistance);
                    if (objectToSpawn != null)
                    {
                        Instantiate(objectToSpawn, spawnPoint, Quaternion.identity);
                    }
                }
            }
        }
    }
}