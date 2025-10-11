using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    enum PlayerState
    {
        Walk,
        Ladder
    };

    [SerializeField] private float speed = 5f;
    [SerializeField] private float climbSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerState pstate;

    private bool isTouchingLadder = false;
    private bool isTouchingFloat = false;
    private Collider2D playerCollider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = rb.GetComponent<Collider2D>();
        pstate = PlayerState.Walk;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // スティックの傾き具合を無視して -1, 0, 1 に変換
        float deadZone = 0.3f; // 小さな傾きを無視するしきい値
        float LadderZone = 0.8f;
        moveInput.x = Mathf.Abs(input.x) < deadZone ? 0 : Mathf.Sign(input.x);
        moveInput.y = Mathf.Abs(input.y) < deadZone ? 0 : Mathf.Sign(input.y);

        if(isTouchingLadder && Mathf.Abs(moveInput.y) > 0)
        {
            SetState(PlayerState.Ladder);
        }
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
            // 左右移動のみ（速度固定）
            velocity.x = moveInput.x * speed;
        }
        else if (pstate == PlayerState.Ladder)
        {
            // はしご中は上下移動も固定速度に
            velocity.x = 0;
            velocity.y = moveInput.y * climbSpeed;
        }

        rb.velocity = velocity;
    }

    public void OnLT(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StageStates.Instance.Reverse();

            var currentStage = StageStates.Instance.CurrentStage;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (currentStage == StageStates.StageState.White)
            {
                spriteRenderer.color = Color.black;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
    private void SetState(PlayerState newState)
    {
        if (pstate == newState) return;
        pstate = newState;

        if(pstate == PlayerState.Ladder)
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
        foreach(var floatObj in GameObject.FindGameObjectsWithTag("Float"))
        {
            var col = floatObj.GetComponent<Collider2D>();
            if(col != null)
            {
                Physics2D.IgnoreCollision(playerCollider, col,ignore);
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
            Debug.Log("Goal");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = false;

            // はしごから離れたら自動で歩行状態に戻す
            if (pstate == PlayerState.Ladder)
                SetState(PlayerState.Walk);
        }
        else if (collision.CompareTag("Float"))
        {
            isTouchingFloat = false;
        }
    }
}
