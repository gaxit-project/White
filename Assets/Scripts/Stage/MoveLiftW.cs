using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LiftMoveW : MonoBehaviour
{
    [SerializeField] private float moveDistance = 2f; // 移動距離
    [SerializeField] private float moveSpeed = 2f;    // 移動速度

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingRight = true;

    private void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.right * moveDistance;
    }

    private void Update()
    {
        // 現在の目的地に向かって移動
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // 到達したら方向を切り替え（2秒ごとに往復するイメージ）
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            movingRight = !movingRight;
            targetPos = movingRight ? startPos + Vector3.right * moveDistance : startPos;
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        // Playerとは衝突を無視する
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }*/
}
