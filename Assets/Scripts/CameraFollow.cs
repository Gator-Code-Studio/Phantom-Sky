using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 3f;
    public float yOffset = 1f;
    public Transform target;

    void LateUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * FollowSpeed);
    }
}