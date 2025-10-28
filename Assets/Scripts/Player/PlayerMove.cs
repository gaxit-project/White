using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    enum PlayerState
    {
        Walk,
        Ladder
    };

    // SwitchState Enum はStageStatesに処理を移管するため、使用する意味が薄くなったため、コメントアウト
    /*
    public enum SwitchState
    {
        On,
        Off
    };
    */

    [SerializeField] private float speed = 5f;
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private Vector3 targetPosition = new Vector3(0f, 0f, 0f);

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerState pstate;
    // private SwitchState sstate; // SwitchStateの削除に伴いコメントアウト

    private bool isTouchingLadder = false;
    private bool isTouchingFloat = false;
    private bool isTouchingSwitch = false;

    // public static bool OnSwitch = false; // StageStatesで状態管理するため削除
    private Collider2D playerCollider;

    // --- Lift 関係 ---
    private LiftBase currentLift = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = rb.GetComponent<Collider2D>();
        pstate = PlayerState.Walk;
        transform.position = targetPosition;
        // sstate = SwitchState.Off; // SwitchStateの削除に伴いコメントアウト
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        float deadZone = 0.3f;
        float LadderZone = 0.8f;

        moveInput.x = Mathf.Abs(input.x) < deadZone ? 0 : Mathf.Sign(input.x);
        moveInput.y = Mathf.Abs(input.y) < deadZone ? 0 : Mathf.Sign(input.y);

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

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;

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
    }

    public void OnLT(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StageStates.Instance.Reverse();

            var currentStage = StageStates.Instance.CurrentStage;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = currentStage == StageStates.StageState.White ? Color.black : Color.white;
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isTouchingSwitch == true)
            {
                // StageStatesのToggleSwitchを呼び出し、状態の反転処理をStageStatesに委譲
                StageStates.Instance.ToggleSwitch();

                // 旧コードのトグル処理は削除
                /*
                if(OnSwitch == false)
                {
                    OnSwitch = true;
                }
                else
                {
                    OnSwitch = false;
                }
                */
            }
        }
    }

    private void SetState(PlayerState newState)
    {
        if (pstate == newState) return;
        pstate = newState;

        if (pstate == PlayerState.Ladder)
        {
            IgnoreFloatCollisions(true);
            rb.gravityScale = 0f;
        }
        else
        {
            IgnoreFloatCollisions(false);
            rb.gravityScale = 1f;
        }
    }

    private void IgnoreFloatCollisions(bool ignore)
    {
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
        }

        if (collision.CompareTag("needle"))
        {
            Debug.Log("Miss");
            transform.position = targetPosition;
        }

        if (collision.CompareTag("Switch"))
        {
            isTouchingSwitch = true;
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

        if (collision.CompareTag("Switch"))
        {
            isTouchingSwitch = false;
        }
    }
}