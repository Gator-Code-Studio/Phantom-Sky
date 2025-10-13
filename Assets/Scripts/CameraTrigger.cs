using UnityEngine;
using System.Collections.Generic;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private bool isDirectional = false;
    public Vector3 newCameraPosition;
    public float moveSpeed = 5f;
    private static Stack<Vector3> camHistory = new Stack<Vector3>();
    private Camera mainCam;
    private bool moveCamera = false;
    private bool hasMoved = false;
    private Vector3 targetPos;

    void Start()
    {
        mainCam = Camera.main;
        if (camHistory.Count == 0) camHistory.Push(mainCam.transform.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (isDirectional)
        {
            float playerX = other.transform.position.x;
            float triggerX = transform.position.x;
            if (playerX < triggerX)
            {
                camHistory.Push(mainCam.transform.position);
                targetPos = new Vector3(newCameraPosition.x, newCameraPosition.y, mainCam.transform.position.z);
            }
            else
            {
                Vector3 back = camHistory.Peek();
                if (camHistory.Count > 1) camHistory.Pop();
                targetPos = new Vector3(back.x, back.y, mainCam.transform.position.z);
            }

            moveCamera = true;
        }
        else
        {
            if (!hasMoved)
            {
                camHistory.Push(mainCam.transform.position);
                targetPos = new Vector3(newCameraPosition.x, newCameraPosition.y, mainCam.transform.position.z);
                moveCamera = true;
                hasMoved = true;
            }
        }
    }

    void Update()
    {
        if (!moveCamera) return;
        mainCam.transform.position =
            Vector3.MoveTowards(mainCam.transform.position, targetPos, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(mainCam.transform.position, targetPos) < 0.05f) moveCamera = false;
    }
}