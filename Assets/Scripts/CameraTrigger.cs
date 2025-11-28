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

    private bool playerInside = false;
    private bool enteredFromLeft = false;

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

            playerInside = true;

            if (playerX < triggerX) enteredFromLeft = true;   // entered from left
            else enteredFromLeft = false;                     // entered from right
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

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!isDirectional) return;
        if (!playerInside) return;

        float playerX = other.transform.position.x;
        float triggerX = transform.position.x;

        // CASE 1: Entered from left, exited to the right -> move forward (push)
        if (enteredFromLeft && playerX > triggerX)
        {
            camHistory.Push(mainCam.transform.position);
            targetPos = new Vector3(newCameraPosition.x, newCameraPosition.y, mainCam.transform.position.z);
            moveCamera = true;
        }
        // CASE 2 (VICE VERSA): Entered from right, exited to the left -> move back (pop)
        else if (!enteredFromLeft && playerX < triggerX)
        {
            if (camHistory.Count > 0)
            {
                Vector3 back = camHistory.Peek();
                if (camHistory.Count > 1) camHistory.Pop();
                targetPos = new Vector3(back.x, back.y, mainCam.transform.position.z);
                moveCamera = true;
            }
        }
        // Any other combo (in left/out left, in right/out right) -> no move

        playerInside = false;
    }

    void Update()
    {
        if (!moveCamera) return;

        mainCam.transform.position =
            Vector3.MoveTowards(mainCam.transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(mainCam.transform.position, targetPos) < 0.05f) moveCamera = false;
    }
}
