using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LiftMoveWW : LiftBase
{
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool movingRight = true;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 previousPos;

    private void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.right * moveDistance;
        previousPos = startPos;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            movingRight = !movingRight;
            targetPos = movingRight ? startPos + Vector3.right * moveDistance : startPos;
        }

        DeltaPosition = transform.position - previousPos;
        previousPos = transform.position;
    }
}
