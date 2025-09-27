using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float wallSlideSpeed = 1f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckRadius = 0.1f;
    [SerializeField] private LayerMask wallLayer;

    private InputAction moveAction;
    private Vector2 moveVector;
    private bool isWallSliding;

    private void Awake()
    {
        moveAction = playerInput.actions["Move"];
    }

    private void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(moveVector.x, 0, 0) * moveSpeed * Time.deltaTime;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }

        WallSlideCheck();
    }

    private void WallSlideCheck()
    {
        bool touchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        if (touchingWall && moveVector.x != 0 && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    
}
