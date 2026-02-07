using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    enum PlayerState
    {
        Walk,
        Ladder
    };


    enum GameState
    {
        On,
        Off
    };

    enum FinishState
    {
        Off,
        On
    };

    [Header("Audio")]
    [SerializeField] private AudioClip goalSound;
    private AudioSource audiosource;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private Vector3 targetPosition = new Vector3(0f, 0f, 0f);
    [SerializeField] private float noMoveTimeLimit = 3f;

    private float noHorizontalInputTimer = 0f;
    private bool noMoveTriggered = false;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerState pstate;
    private GameState gstate;
    private FinishState fstate;
    private CameraZoom cameraZoom;
    private TimeManager timeManager;
    // private PlayerInput playerInput; // ❌ 削除: PlayerInputの参照

    private bool isTouchingLadder = false;
    private bool isTouchingFloat = false;
    private bool isTouchingSwitch = false;

    public static event Action OnPlayerFinished;

    private Collider2D playerCollider;

    // --- Lift 関係 ---
    private LiftBase currentLift = null;

    public static event Action OnNoHorizontalInputFor3Seconds;
    public static event Action OnHorizontalInputResumed;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // playerInput = GetComponent<PlayerInput>(); // ❌ 削除: PlayerInputの取得
        playerCollider = rb.GetComponent<Collider2D>();
        pstate = PlayerState.Walk;
        transform.position = targetPosition;
        gstate = GameState.On;
        fstate = FinishState.Off;
    }

    private void Start()
    {
        audiosource = gameObject.GetComponent<AudioSource>();
        audiosource.playOnAwake = false;

        if (Camera.main != null)
        {
            cameraZoom = Camera.main.GetComponent<CameraZoom>();
        }
        if (cameraZoom != null)
        {
            cameraZoom.ZoomOutImmediate();
        }

        timeManager = FindObjectOfType<TimeManager>();
        if (timeManager == null)
        {
            Debug.LogError("No TimeManager");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (gstate == GameState.On)
        {
            Vector2 input = context.ReadValue<Vector2>();

            float deadZone = 0.3f;
            float LadderZone = 0.8f;

            moveInput.x = Mathf.Abs(input.x) < deadZone ? 0 : Mathf.Sign(input.x);
            moveInput.y = Mathf.Abs(input.y) < deadZone ? 0 : Mathf.Sign(input.y);

            if (moveInput.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveInput.x);
                transform.localScale = scale;

                noHorizontalInputTimer = 0f;

                // 🔹 無操作状態から復帰した瞬間
                if (noMoveTriggered)
                {
                    noMoveTriggered = false;
                    OnHorizontalInputResumed?.Invoke();
                }
            }

            // はしごに触れていて縦入力 → はしご状態へ
            if (isTouchingLadder && Mathf.Abs(moveInput.y) > 0)
            {
                SetState(PlayerState.Ladder);
            }

            // 強く横に倒したら Walk に戻す
            if (!isTouchingFloat && pstate == PlayerState.Ladder && Mathf.Abs(input.x) > LadderZone)
            {
                SetState(PlayerState.Walk);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;

        // 🎯 Floatとの衝突制御は維持 🎯
        bool ignoreFloat = (pstate == PlayerState.Ladder);
        IgnoreFloatCollisions(ignoreFloat);

        if (pstate == PlayerState.Walk)
        {
            velocity.x = moveInput.x * speed;
        }
        else if (pstate == PlayerState.Ladder)
        {
            velocity.x = 0;
            velocity.y = moveInput.y * climbSpeed;
        }

        rb.velocity = velocity;

        // --- Lift追従処理 ---
        if (currentLift != null)
        {
            transform.position += currentLift.DeltaPosition;
        }

        if (gstate != GameState.On) return;

        // 左右入力が無い状態
        if (moveInput.x == 0)
        {
            noHorizontalInputTimer += Time.deltaTime;

            if (noHorizontalInputTimer >= noMoveTimeLimit && !noMoveTriggered)
            {
                noMoveTriggered = true;
                OnNoHorizontalInputFor3Seconds?.Invoke();
            }
        }
    }

    public void OnLT(InputAction.CallbackContext context)
    {
        if (gstate == GameState.On && context.performed)
        {
            StageStates.Instance.Reverse();

            var currentStage = StageStates.Instance.CurrentStage;

            var parentSR = GetComponent<SpriteRenderer>();
            var childSRs = GetComponentsInChildren<SpriteRenderer>();

            foreach (var sr in childSRs)
            {
                if (sr == parentSR) continue; // 親は別処理

                sr.color = currentStage == StageStates.StageState.White ? Color.white : Color.black;
            }

            parentSR.color = currentStage == StageStates.StageState.White ? Color.black : Color.white;
        }
    }

    /*public void OnAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isTouchingSwitch == true)
            {
                StageStates.Instance.ToggleSwitch();
            }
        }
    }*/

    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (fstate == FinishState.Off)
            {
                PauseManager.Instance.TogglePause();
            }
        }
    }

    public void SetGameState(bool on)
    {
        gstate = on ? GameState.On : GameState.Off;
        moveInput = Vector2.zero; // 入力リセット
        rb.velocity = Vector2.zero; // 慣性リセット
    }


    public void ResetInput()
    {
        moveInput = Vector2.zero;
        rb.velocity = Vector2.zero; // 慣性も止める
    }

    private void SetState(PlayerState newState)
    {
        if (pstate == newState) return;
        pstate = newState;

        if (pstate == PlayerState.Ladder)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void IgnoreFloatCollisions(bool ignore)
    {
        // Floatタグを持つすべてのオブジェクトとの衝突を無視/有効化
        foreach (var floatObj in GameObject.FindGameObjectsWithTag("Float"))
        {
            var col = floatObj.GetComponent<Collider2D>();
            if (col != null)
            {
                Physics2D.IgnoreCollision(playerCollider, col, ignore);
            }
        }
    }

    // --- Lift検出 ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lift"))
        {
            var lift = collision.gameObject.GetComponent<LiftBase>();
            if (lift != null)
            {
                currentLift = lift;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lift"))
        {
            if (currentLift != null && collision.transform == currentLift.transform)
            {
                currentLift = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = true;
        }
        else if (collision.CompareTag("Float"))
        {
            isTouchingFloat = true;
        }

        if (collision.CompareTag("Goal"))
        {
            Debug.Log("Goal!");
            gstate = GameState.Off;
            fstate = FinishState.On;

            rb.velocity = Vector2.zero;
            moveInput = Vector2.zero;
            // if (playerInput != null) { playerInput.enabled = false; } // ❌ 削除: 入力無効化

            OnPlayerFinished?.Invoke();

            int clearTime = 0;

            if(goalSound != null)
            {
                audiosource.PlayOneShot(goalSound);
            }

            if (Camera.main != null)
            {
                cameraZoom.FocusOnPosition(transform.position);
            }

            if (timeManager != null)
            {
                clearTime = timeManager.StopTimer();
            }

            if (StageStates.Instance != null)
            {
                StageStates.Instance.ShowClearUI(clearTime);
            }
        }

        if (collision.CompareTag("needle"))
        {
            Debug.Log("Miss");
            // 🎯 復活: 以前のワープ処理に戻す 🎯
            transform.position = targetPosition;
        }

        if (collision.CompareTag("Switch"))
        {
            if(isTouchingSwitch == false)
            {
                StageStates.Instance.ToggleSwitch();
                isTouchingSwitch = true;
            }            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = false;

            if (pstate == PlayerState.Ladder)
                SetState(PlayerState.Walk);
        }
        else if (collision.CompareTag("Float"))
        {
            isTouchingFloat = false;
        }

        /*if (collision.CompareTag("Switch"))
        {
            isTouchingSwitch = false;
        }*/
    }
}