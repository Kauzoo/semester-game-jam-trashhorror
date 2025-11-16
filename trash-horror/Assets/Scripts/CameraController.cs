using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float smoothTime = 0.5f;

    private Vector3 velocity = new Vector3();

    void Update()
    {
        Vector3 newPos = Vector3.SmoothDamp(transform.position, target.transform.position, ref velocity, smoothTime);
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
