using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public Vector3 newCameraPosition;
    public float moveSpeed = 5f;

    private Camera mainCam;
    private bool moveCamera = false;

    void Start()
    {
        mainCam = Camera.main;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            moveCamera = true;
    }

    void Update()
    {
        if (moveCamera)
        {
            mainCam.transform.position = Vector3.MoveTowards(
                mainCam.transform.position,
                new Vector3(newCameraPosition.x, newCameraPosition.y, mainCam.transform.position.z),
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(mainCam.transform.position, newCameraPosition) < 0.05f)
                moveCamera = false;
        }
    }
}