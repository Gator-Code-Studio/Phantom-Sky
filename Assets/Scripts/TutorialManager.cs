using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public enum StepType
    {
        Info,
        MoveRight,
        MoveLeft,
        JumpOnce,
        JumpDouble,
        Dash,
        ThrowShuriken,
        KatanaAttack,
        WallJump,
        KillEnemy,
        JumpPad,
        AvoidSpikes,
        UseTeleporter,
        CollectItem,
        UsePortal
    }

    [System.Serializable]
    public class Step
    {
        public StepType type;
        public string message;
    }

    public List<Step> steps = new List<Step>();
    public CanvasGroup panel;
    public TMP_Text messageText;
    public KeyCode skipKey = KeyCode.Escape;

    int index;
    bool waitingForAnyKey;
    bool running;
    bool subscribed;

    void OnEnable()
    {
        TrySubscribe();
    }

    void Start()
    {
        SeedIfEmpty();
        if (steps.Count > 0)
        {
            running = true;
            ShowCurrent();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(skipKey)) { End(); return; }
        if (!running) { return; }
        if (!subscribed) { TrySubscribe(); }
        if (waitingForAnyKey && Input.anyKeyDown) { waitingForAnyKey = false; Next(); }
    }

    void TrySubscribe()
    {
        var r = PlayerActionReporter.Instance;
        if (r == null || subscribed) { return; }

        r.OnMoveRight += OnMoveRight;
        r.OnMoveLeft += OnMoveLeft;
        r.OnJumpOnce += OnJumpOnce;
        r.OnJumpDouble += OnJumpDouble;
        r.OnDash += OnDash;
        r.OnThrowShuriken += OnThrowShuriken;
        r.OnKatanaAttack += OnKatanaAttack;
        r.OnEnemyKilled += OnEnemyKilled;
        r.OnWallJump += OnWallJump;
        r.OnJumpPadUsed += OnJumpPadUsed;
        r.OnSpikeSectionCleared += OnSpikeSectionCleared;
        r.OnTeleporterUsed += OnTeleporterUsed;
        r.OnCollectiblePicked += OnCollectiblePicked;
        r.OnPortalUsed += OnPortalUsed;

        subscribed = true;
    }

    void OnDisable()
    {
        var r = PlayerActionReporter.Instance;
        if (r != null)
        {
            r.OnMoveRight -= OnMoveRight;
            r.OnMoveLeft -= OnMoveLeft;
            r.OnJumpOnce -= OnJumpOnce;
            r.OnJumpDouble -= OnJumpDouble;
            r.OnDash -= OnDash;
            r.OnThrowShuriken -= OnThrowShuriken;
            r.OnKatanaAttack -= OnKatanaAttack;
            r.OnEnemyKilled -= OnEnemyKilled;
            r.OnWallJump -= OnWallJump;
            r.OnJumpPadUsed -= OnJumpPadUsed;
            r.OnSpikeSectionCleared -= OnSpikeSectionCleared;
            r.OnTeleporterUsed -= OnTeleporterUsed;
            r.OnCollectiblePicked -= OnCollectiblePicked;
            r.OnPortalUsed -= OnPortalUsed;
        }
        subscribed = false;
    }

    void ShowCurrent()
    {
        if (index < 0) { index = 0; }
        if (index >= steps.Count) { End(); return; }

        var s = steps[index];
        messageText.text = s.message;
        StopAllCoroutines();
        StartCoroutine(Fade(panel, 0f, 1f, 0.15f));
        waitingForAnyKey = s.type == StepType.Info;
    }

    void Next()
    {
        index = index + 1;
        ShowCurrent();
    }

    void CompleteIf(StepType t)
    {
        if (!running || index >= steps.Count) { return; }
        if (steps[index].type == t) { Next(); }
    }

    void OnMoveRight() { CompleteIf(StepType.MoveRight); }
    void OnMoveLeft() { CompleteIf(StepType.MoveLeft); }
    void OnJumpOnce() { CompleteIf(StepType.JumpOnce); }
    void OnJumpDouble() { CompleteIf(StepType.JumpDouble); }
    void OnDash() { CompleteIf(StepType.Dash); }
    void OnThrowShuriken() { CompleteIf(StepType.ThrowShuriken); }
    void OnKatanaAttack() { CompleteIf(StepType.KatanaAttack); }
    void OnEnemyKilled() { CompleteIf(StepType.KillEnemy); }
    void OnWallJump() { CompleteIf(StepType.WallJump); }
    void OnJumpPadUsed() { CompleteIf(StepType.JumpPad); }
    void OnSpikeSectionCleared() { CompleteIf(StepType.AvoidSpikes); }
    void OnTeleporterUsed() { CompleteIf(StepType.UseTeleporter); }
    void OnCollectiblePicked() { CompleteIf(StepType.CollectItem); }
    void OnPortalUsed() { CompleteIf(StepType.UsePortal); }

    IEnumerator Fade(CanvasGroup g, float a, float b, float d)
    {
        float t = 0f;
        g.blocksRaycasts = true;
        while (t < d)
        {
            t = t + Time.unscaledDeltaTime;
            g.alpha = Mathf.Lerp(a, b, t / d);
            yield return null;
        }
        g.alpha = b;
    }

    void End()
    {
        running = false;
        StopAllCoroutines();
        StartCoroutine(Fade(panel, panel.alpha, 0f, 0.15f));
        panel.blocksRaycasts = false;
        PlayerPrefs.SetInt("tutorial_done", 1);
        PlayerPrefs.Save();
    }

    void SeedIfEmpty()
    {
        if (steps.Count > 0) { return; }
        steps.Add(new Step { type = StepType.MoveRight, message = "Press D to move right." });
        steps.Add(new Step { type = StepType.MoveLeft, message = "Press A to move left." });
        steps.Add(new Step { type = StepType.JumpOnce, message = "Press Space to jump." });
        steps.Add(new Step { type = StepType.JumpDouble, message = "Press Space twice to double jump." });
        steps.Add(new Step { type = StepType.Dash, message = "Hold Shift and a direction to dash." });
        steps.Add(new Step { type = StepType.ThrowShuriken, message = "Press F to throw a shuriken." });
        steps.Add(new Step { type = StepType.KatanaAttack, message = "Press J for katana attack." });
        steps.Add(new Step { type = StepType.KillEnemy, message = "Kill the enemy." });
        steps.Add(new Step { type = StepType.WallJump, message = "Press Space while touching a wall to wall jump." });
        steps.Add(new Step { type = StepType.JumpPad, message = "Use the jump pad to launch upward. Avoid the spikes." });
        steps.Add(new Step { type = StepType.UsePortal, message = "Enter the portal to collect the item." });
        steps.Add(new Step { type = StepType.CollectItem, message = "Collect the item." });
        steps.Add(new Step { type = StepType.UseTeleporter, message = "Go through this teleporter to proceed to the next level." });
    }
}
