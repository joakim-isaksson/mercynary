using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;
    public float CameraSpeed = 4.0f;

    public GameObject FollowObject;

    private float targetX;
    private float targetY;

    void FixedUpdate()
    {
        float cameraX = transform.position.x;
        float cameraY = transform.position.y;
        float cameraZ = transform.position.z;

        targetX = FollowObject.transform.position.x;
        targetY = FollowObject.transform.position.y;

        cameraX = Mathf.Clamp(Mathf.Lerp(cameraX, targetX, Time.deltaTime * CameraSpeed), MinX, MaxX);
        cameraY = Mathf.Clamp(Mathf.Lerp(cameraY, targetY, Time.deltaTime * CameraSpeed), MinY, MaxY);

        transform.position = new Vector3(cameraX, cameraY, cameraZ);
    }
}
