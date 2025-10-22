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

    public enum SwitchState
    {
        On,
        Off
    };

    [SerializeField] private float speed = 5f;
    [SerializeField] private float climbSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerState pstate;
    private SwitchState sstate;

    private bool isTouchingLadder = false;
    private bool isTouchingFloat = false;
    private bool isTouchingSwitch = false;
    private Collider2D playerCollider;

    // --- Lift ä÷åW ---
    private LiftBase currentLift = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = rb.GetComponent<Collider2D>();
        pstate = PlayerState.Walk;
        sstate = SwitchState.Off;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        float deadZone = 0.3f;
        float LadderZone = 0.8f;

        moveInput.x = Mathf.Abs(input.x) < deadZone ? 0 : Mathf.Sign(input.x);
        moveInput.y = Mathf.Abs(input.y) < deadZone ? 0 : Mathf.Sign(input.y);

        // ÇÕÇµÇ≤Ç…êGÇÍÇƒÇ¢Çƒècì¸óÕ Å® ÇÕÇµÇ≤èÛë‘Ç÷
        if (isTouchingLadder && Mathf.Abs(moveInput.y) > 0)
        {
            SetState(PlayerState.Ladder);
        }

        // ã≠Ç≠â°Ç…ì|ÇµÇΩÇÁ Walk Ç…ñﬂÇ∑
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

        // --- Liftí«è]èàóù ---
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
        if(context.performed)
        {
            if(isTouchingSwitch == true)
            {
                if(sstate == SwitchState.Off)
                {
                    sstate = SwitchState.On;
                }
                else if(sstate == SwitchState.On)
                {
                    sstate = SwitchState.Off;
                }
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

    // --- Liftåüèo ---
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
