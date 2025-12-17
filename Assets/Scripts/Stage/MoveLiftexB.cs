using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLiftexB : LiftBase
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

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Åö StageÇ™WhiteÇ≈Ç»ÇØÇÍÇŒâΩÇ‡ÇµÇ»Ç¢
        if (StageStates.Instance == null ||
            StageStates.Instance.CurrentStage != StageStates.StageState.Black)
        {
            DeltaPosition = Vector3.zero;
            previousPos = transform.position;
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.fixedDeltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            movingRight = !movingRight;
            targetPos = movingRight
                ? startPos + Vector3.right * moveDistance
                : startPos;
        }

        DeltaPosition = transform.position - previousPos;
        previousPos = transform.position;
    }

}
