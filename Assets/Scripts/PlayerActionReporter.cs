using UnityEngine;
using System;

public class PlayerActionReporter : MonoBehaviour
{
    public static PlayerActionReporter Instance;

    public event Action OnMoveRight;
    public event Action OnMoveLeft;
    public event Action OnJumpOnce;
    public event Action OnJumpDouble;
    public event Action OnDash;
    public event Action OnThrowShuriken;
    public event Action OnKatanaAttack;
    public event Action OnEnemyKilled;
    public event Action OnWallJump;
    public event Action OnJumpPadUsed;
    public event Action OnSpikeSectionCleared;
    public event Action OnTeleporterUsed;
    public event Action OnCollectiblePicked;
    public event Action OnPortalUsed;

    float jumpWindow = 0.35f;
    int jumpCount;
    float jumpTimer;
    bool dashedThisPress;
    bool movedRightFired;
    bool movedLeftFired;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!movedRightFired && Input.GetKeyDown(KeyCode.D))
        {
            movedRightFired = true;
            if (OnMoveRight != null) { OnMoveRight(); }
        }

        if (!movedLeftFired && Input.GetKeyDown(KeyCode.A))
        {
            movedLeftFired = true;
            if (OnMoveLeft != null) { OnMoveLeft(); }
        }

        if (Input.GetKeyDown(KeyCode.F)) { if (OnThrowShuriken != null) { OnThrowShuriken(); } }
        if (Input.GetKeyDown(KeyCode.J)) { if (OnKatanaAttack != null) { OnKatanaAttack(); } }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpTimer > 0f) { jumpCount = jumpCount + 1; }
            else { jumpCount = 1; }
            jumpTimer = jumpWindow;
            if (jumpCount == 1) { if (OnJumpOnce != null) { OnJumpOnce(); } }
            else if (jumpCount == 2) { if (OnJumpDouble != null) { OnJumpDouble(); } }
        }

        if (jumpTimer > 0f)
        {
            jumpTimer = jumpTimer - Time.unscaledDeltaTime;
            if (jumpTimer <= 0f) { jumpCount = 0; }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) { dashedThisPress = false; }
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && !dashedThisPress)
        {
            dashedThisPress = true;
            if (OnDash != null) { OnDash(); }
        }
    }

    public void ReportWallJump() { if (OnWallJump != null) { OnWallJump(); } }
    public void ReportEnemyKilled() { if (OnEnemyKilled != null) { OnEnemyKilled(); } }
    public void ReportJumpPadUsed() { if (OnJumpPadUsed != null) { OnJumpPadUsed(); } }
    public void ReportSpikeSectionCleared() { if (OnSpikeSectionCleared != null) { OnSpikeSectionCleared(); } }
    public void ReportTeleporterUsed() { if (OnTeleporterUsed != null) { OnTeleporterUsed(); } }
    public void ReportCollectiblePicked() { if (OnCollectiblePicked != null) { OnCollectiblePicked(); } }
    public void ReportPortalUsed() { if (OnPortalUsed != null) { OnPortalUsed(); } }
}
