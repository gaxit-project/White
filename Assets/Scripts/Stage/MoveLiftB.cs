using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LiftMoveB : LiftBase
{
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingUp = true;
    private Vector3 previousPos;

    private void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.up * moveDistance;
        previousPos = startPos;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            movingUp = !movingUp;
            targetPos = movingUp ? startPos + Vector3.up * moveDistance : startPos;
        }

        DeltaPosition = transform.position - previousPos;
        previousPos = transform.position;
    }
}
